using Agrigate.Api.Core.Messages;
using Agrigate.Api.Core.Queries;
using Agrigate.Api.Crops.Messages;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities.Crops;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Api.Crops.Actors;

/// <summary>
/// The root actor for interacting with crops
/// </summary>
public class CropManager : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggingAdapter _logger;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CropManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = Context.GetLogger<SerilogLoggingAdapter>();

        ReceiveAsync<CropQueries.QueryCropDetail>(QueryCropDetail);
        ReceiveAsync<CropCommands.CreateCropDetail>(CreateCropDetail);
    }

    private async Task QueryCropDetail(CropQueries.QueryCropDetail query)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            await using var db = scope.ServiceProvider.GetRequiredService<AgrigateContext>();
        
            var result = await db.CropDetails
                .AsNoTracking()
                .ToPaginatedResult(
                    query.Params.Page,
                    query.Params.PageSize,
                    query.Uri.Value,
                    CancellationToken.None);
        
            Sender.Tell(result);
        }
        catch (Exception ex)
        {
            var innermostException = ex;
            while (innermostException.InnerException != null)
                innermostException = innermostException.InnerException;
            
            _logger.Error(ex, "Error retrieving CropDetail: {Message}", ex.Message);
            
            Sender.Tell(new CoreEvents.UnexpectedError("Unable to retrieve CropDetail. Please try again."));
        }
    }
    
    /// <summary>
    /// Creates a new CropDetail record
    /// </summary>
    /// <param name="command"></param>
    private async Task CreateCropDetail(CropCommands.CreateCropDetail command)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            await using var db = scope.ServiceProvider.GetRequiredService<AgrigateContext>();
        
            var duplicateCrop = await db.CropDetails
                .AsNoTracking()
                .Where(c => 
                    c.Crop == command.Detail.Crop.Value
                    && c.Cultivar == command.Detail.Cultivar
                )
                .FirstOrDefaultAsync(CancellationToken.None);

            if (duplicateCrop != null)
            {
                var cultivarName = command.Detail.Cultivar;
                var cropName = command.Detail.Crop.Value;
            
                _logger.Warning(
                    "Duplicate CropDetail detected for {0} {1}",
                    cultivarName,
                    cropName
                );
            
                Sender.Tell(new CoreEvents.ValidationError($"Duplicate CropDetail detected for {cultivarName} {cropName}"));
                return;
            }
        
            var newCropDetail = new CropDetail
            {
                Crop = command.Detail.Crop.Value,
                Cultivar = command.Detail.Cultivar,
                Dtm = command.Detail.Dtm.Value,
                StackingDays = command.Detail.StackingDays.Value,
                BlackoutDays = command.Detail.BlackoutDays.Value,
                NurseryDays = command.Detail.NurseryDays.Value,
                SoakHours = command.Detail.SoakHours.Value,
                PlantQuantity = command.Detail.PlantQuantity.Value
            };

            db.Add(newCropDetail);
            await db.SaveChangesAsync(CancellationToken.None);
        
            Sender.Tell(newCropDetail);
        }
        catch (Exception ex)
        {
            var innermostException = ex;
            while (innermostException.InnerException != null)
                innermostException = innermostException.InnerException;
            
            _logger.Error(ex, "Error creating CropDetail: {Message}", ex.Message);
            
            Sender.Tell(new CoreEvents.UnexpectedError("Unable to create CropDetail. Please try again."));
        }
    }
}
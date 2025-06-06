using System.Net;
using Agrigate.Api.Core.Messages;
using Agrigate.Api.Core.Queries;
using Agrigate.Api.Core.ValueTypes;
using Agrigate.Api.Crops.Actors;
using Agrigate.Api.Crops.Messages;
using Agrigate.Api.Crops.Models;
using Agrigate.Domain.Entities.Crops;
using Agrigate.Domain.Models;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Crops.Controllers;

/// <summary>
/// Controller for handling Crop-related requests
/// </summary>
[ApiController]
[Tags("Crops")]
[Route("Crops")]
public class CropController : ControllerBase
{
    private readonly IActorRef _cropManager;
    private readonly ILogger<CropController> _logger;
    
    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="registry"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public CropController(
        ILogger<CropController> logger,
        ActorRegistry registry
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        if (!registry.TryGet<CropManager>(out _cropManager))
            throw new ApplicationException($"{nameof(CropManager)} has not been registered");
    }

    #region Crop Details
    
    /// <summary>
    /// Retrieve Crop Details
    /// </summary>
    /// <param name="queryParams">The query parameters, if any</param>
    /// <returns>A CropDetail record</returns>
    /// <response code="200">Request Successful</response>
    [HttpGet("Detail")]
    [ProducesResponseType(typeof(PaginatedResult<CropDetail>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetCropDetail([FromQuery] QueryParams queryParams)
    {
        try
        {
            var query = new CropQueries.QueryCropDetail(
                new NonEmptyString(nameof(CropQueries.QueryCropDetail.Uri), Request.GetDisplayUrl()), 
                queryParams
            );
            var result = await _cropManager.Ask(query, TimeSpan.FromSeconds(5));

            if (result is IErrorEvent error)
                throw new ApplicationException(error.Message);
            
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Create a new CropDetail record
    /// </summary>
    /// <param name="request">The details to save</param>
    /// <returns></returns>
    [HttpPost("Detail")]
    [ProducesResponseType(typeof(CropDetail), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateCropDetail([FromBody] CropDetailBase request)
    {
        try
        {
            var newDetail = new NewCropDetail(
                new NonEmptyString(nameof(NewCropDetail.Crop), request.Crop),
                request.Cultivar,
                new PositiveInt(nameof(NewCropDetail.Dtm), request.Dtm),
                new NullOrNonNegativeInt(nameof(NewCropDetail.StackingDays), request.StackingDays),
                new NullOrNonNegativeInt(nameof(NewCropDetail.BlackoutDays), request.BlackoutDays),
                new NullOrNonNegativeInt(nameof(NewCropDetail.NurseryDays), request.NurseryDays),
                new NullOrNonNegativeInt(nameof(NewCropDetail.SoakHours), request.SoakHours),
                new NullOrPositiveDouble(nameof(NewCropDetail.PlantQuantity), request.PlantQuantity)
            );

            var command = new CropCommands.CreateCropDetail(newDetail);
            var result = await _cropManager.Ask(command, TimeSpan.FromSeconds(5));

            if (result is IErrorEvent error)
                throw error is CoreEvents.ValidationError
                    ? new ArgumentException(error.Message)
                    : new ApplicationException(error.Message);

            return CreatedAtAction(nameof(GetCropDetail), result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError); 
        }
    }
    
    #endregion
}
using Agrigate.Domain.Entities.Crops;
using Agrigate.Domain.Models;
using Agrigate.Web.Services.AgrigateApiClient;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Agrigate.Web.Components.Dialogs.EditCropDetail;

public class EditCropDetailBase : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance Dialog { get; set; }
    
    [Inject]
    public required ILogger<EditCropDetailBase> Logger { get; set; }
    
    [Inject]
    public required IAgrigateApiClient ApiClient { get; set; }

    protected CropDetail Detail { get; set; } = new();
    protected bool IsLoading { get; set; }
    
    protected void HandleClose()
    {
        Dialog.Close();
    }

    protected async Task HandleSave()
    {
        try
        {
            IsLoading = true;

            var newDetail = new CropDetailBase(
                Detail.Crop,
                Detail.Cultivar,
                Detail.Dtm,
                Detail.StackingDays,
                Detail.BlackoutDays,
                Detail.NurseryDays,
                Detail.SoakHours,
                Detail.PlantQuantity
            );

            var result = await ApiClient.Request<CropDetail>(
                HttpMethod.Post,
                "/Crops/Detail",
                body: newDetail
            );

            Dialog.Close(DialogResult.Ok(result));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating CropDetail: {Message}", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
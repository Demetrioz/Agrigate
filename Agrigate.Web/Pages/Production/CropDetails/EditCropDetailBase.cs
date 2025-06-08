using Agrigate.Domain.Entities.Crops;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Agrigate.Web.Pages.Production.CropDetails;

public class EditCropDetailBase : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance Dialog { get; set; }

    protected CropDetail Detail { get; set; } = new();
    
    protected void HandleClose()
    {
        Dialog.Close();
    }

    protected void HandleSave()
    {
        Dialog.Close();
    }
}
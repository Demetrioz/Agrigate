using Agrigate.Domain.Entities.Crops;
using Agrigate.Domain.Models.Queries;
using Agrigate.Web.Components.Dialogs.EditCropDetail;
using Agrigate.Web.Services.AgrigateApiClient;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Agrigate.Web.Pages.Production.CropDetails;

public class CropDetailsBase : ComponentBase
{
    [Inject]
    public required ILogger<CropDetailsBase> Logger { get; set; }
    
    [Inject]
    public required IAgrigateApiClient ApiClient { get; set; }
    
    [Inject]
    public required IDialogService DialogService { get; set; }

    protected readonly List<BreadcrumbItem> Breadcrumbs =
    [
        new ("Home", href: "/"),
        new ("Production", href: null, disabled: true),
        new ("Crop Details", href: null, disabled: true)
    ];
    
    protected bool IsLoading { get; private set; } = true;
    protected string SearchTerm { get; set; } = "";
    protected List<CropDetail> CropDetails { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var results = await ApiClient.Request<PaginatedResult<CropDetail>>(
                HttpMethod.Get, 
                "/Crops/Detail",
                // TODO: Implement appropriate paging and sorting for MudTable
                queryParams: new Dictionary<string, string>
                {
                    { "Page", "1" },
                    { "PageSize", "100" }
                }
            );
            CropDetails = results.Items;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving CropDetails: {Message}", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Determines which CropDetailRecords are available in the table when filtering 
    /// </summary>
    /// <param name="detail"></param>
    /// <returns></returns>
    protected bool Filter(CropDetail detail)
    {
        return string.IsNullOrWhiteSpace(SearchTerm)
            || detail.Crop.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
            || (detail.Cultivar ?? "").Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Displays the EditCropDetail dialog
    /// </summary>
    protected async Task HandleCreateNewDetail()
    {
        var dialog = await DialogService.ShowAsync<EditCropDetail>("Create Crop Detail");
        var result = await dialog.Result;

        if (result == null || result.Canceled || result.Data is not CropDetail typedResult)
            return;
        
        CropDetails.Add(typedResult);
    }
}
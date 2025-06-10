using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Agrigate.Web.Pages.Production.Crops;

public class CropsBase : ComponentBase
{
    [Inject]
    public required ILogger<CropsBase> Logger { get; set; }

    protected readonly List<BreadcrumbItem> Breadcrumbs = 
    [
        new ("Home", href: "/"),
        new ("Production", href: null, disabled: true),
        new ("Crops", href: null, disabled: true)
    ];
    
    protected bool IsLoading { get; private set; } = true;

    protected override Task OnInitializedAsync()
    {
        try
        {
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving Crops: {Message}", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
        
        return base.OnInitializedAsync();
    }
}
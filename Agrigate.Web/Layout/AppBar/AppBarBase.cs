using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Agrigate.Web.Layout.AppBar;

public class AppBarBase : ComponentBase
{
    [Parameter]
    public required Action OnToggle { get; set; }
    
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected string CurrentUrl { get; private set; } = "";

    protected override void OnInitialized()
    {
        CurrentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        CurrentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
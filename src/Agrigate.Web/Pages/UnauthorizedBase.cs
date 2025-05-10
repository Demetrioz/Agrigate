using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Agrigate.Web.Pages;

public class UnauthorizedBase : ComponentBase
{
    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }
    
    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    
    [Inject]
    public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var isAuthenticated = authState.User.Identity?.IsAuthenticated ?? false;

        var forceLoad = !isAuthenticated;
        var finalUrl = !isAuthenticated
            ? $"authentication/login?returnUrl={Uri.EscapeDataString(ReturnUrl ?? "/")}"
            : "/";
        
        NavigationManager.NavigateTo(finalUrl, forceLoad);
    }
}
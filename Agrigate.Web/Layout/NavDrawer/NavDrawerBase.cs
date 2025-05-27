using Microsoft.AspNetCore.Components;

namespace Agrigate.Web.Layout.NavDrawer;

public class NavDrawerBase : ComponentBase
{
    [Parameter]
    public bool IsOpen { get; set; }
}
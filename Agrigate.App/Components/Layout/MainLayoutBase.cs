using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Agrigate.App.Components.Layout;

public class MainLayoutBase : LayoutComponentBase
{
    protected bool DrawerOpen { get; set; }

    protected MudTheme AgrigateTheme = new MudTheme
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#60b33d",
            Secondary = "#11bcc9",
            AppbarBackground = "#44862a",
            Background = "#dfdfdf",
            Error = "#ff3838",
            Warning = "#ecb206",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#60b33d",
            Secondary = "#11bcc9",
            AppbarBackground = "#44862a",
            Background = "#dfdfdf",
            Error = "#ff3838",
            Warning = "#ecb206",
        }
    };
}
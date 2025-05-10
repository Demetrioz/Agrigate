using Microsoft.AspNetCore.Components;

namespace Agrigate.Web.Pages.Test;

public class TestBase : ComponentBase
{
    [Inject]
    public required ILogger<TestBase> Logger { get; set; }

    protected override void OnInitialized()
    {
        Logger.LogInformation("Test initialized");
    }
}
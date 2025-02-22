using Microsoft.AspNetCore.Components;

namespace Agrigate.App.Components.Pages;

public class CounterBase : ComponentBase
{
    protected int CurrentCount { get; private set; } = 0;
    
    protected void HandleClick()
    {
        CurrentCount++;
    }
}
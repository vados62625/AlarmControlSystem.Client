using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Extensions;

public static class NavigationManagerExtensions
{
    public static void ReloadPage(this NavigationManager manager)
    {
        manager.NavigateTo(manager.Uri, true);
    }
}
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;


namespace GPNA.AlarmControlSystem.Shared
{
    public class SpinnerComponentModel : ComponentBase
    {
        [Inject] public ISpinnerService SpinnerService { get; set; } = default!;
        public bool IsVisible { get; set; }

        protected override void OnInitialized()
        {
            SpinnerService.OnShow += ShowLoadingSpinner;
            SpinnerService.OnHide += HideLoadingSpinner;
        }

        private void ShowLoadingSpinner()
        {
            IsVisible = true;
            if (IsVisible == true)
                StateHasChanged();
        }

        private async void HideLoadingSpinner()
        {
            IsVisible = false;
            await Task.Delay(200);

            if (IsVisible == false)
                StateHasChanged();
        }
    }
}

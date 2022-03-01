using Client.Infrastructure.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace Client.App.Shared
{
    public partial class MainLayout
    {
        private bool IsAuthenticated { get; set; }
        private bool DrawerOpen { get; set; } = true;
        private MudTheme CurrentTheme { get; set; }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            IsAuthenticated = user != null && user.Identity?.IsAuthenticated == true;
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
                StateHasChanged();

                if (!IsAuthenticated)
                {
                    _navigationManager.NavigateTo("/", true);
                }
            }
        }

        protected override void OnInitialized()
        {
            CurrentTheme = _clientPreferenceManager.GetCurrentTheme();
            FetchDataExecutor.StartExecuting();
            RenderWalletExecutor.StartExecuting();
        }

        private void DrawerToggle()
        {
            DrawerOpen = !DrawerOpen;
        }
    }
}
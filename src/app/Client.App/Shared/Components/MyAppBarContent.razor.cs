using Application.CreatorPortal.Account.Queries.GetWallet;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Constants;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Shared.Components
{
    public partial class MyAppBarContent : IDisposable
    {
        [Inject] public IAccountManager AccountManager { get; set; }

        private string Name { get; set; }
        private string WalletAddress { get; set; }
        private string WalletBalance { get; set; }
        private bool IsAccountValid { get; set; }

        private bool IsPopulateWalletProcessing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            RenderWalletExecutor.JobExecuted += HandleJobExecuted;
        }

        public void Dispose()
        {
            RenderWalletExecutor.JobExecuted -= HandleJobExecuted;
        }

        private async void HandleJobExecuted(object sender, GetWalletResponse e)
        {
            await RenderWallet(e);
        }

        private async Task LoadDataAsync()
        {
            var profile = await AccountManager.FetchProfileAsync();

            if (profile != null)
            {
                Name = profile.Name;
            }

            var wallet = await AccountManager.FetchWalletAsync();

            if (wallet != null)
            {
                await RenderWallet(wallet);
            }
        }

        private async Task RenderWallet(GetWalletResponse wallet)
        {
            IsAccountValid = wallet.IsAccountValid;
            WalletAddress = wallet.AccountAddress;
            WalletBalance = $"{wallet.Balance:N6} XRP";
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnPopulateWalletAsync()
        {
            IsPopulateWalletProcessing = true;

            try
            {
                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.PopulateWalletAsync());
                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                await LoadDataAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            IsPopulateWalletProcessing = false;
        }

        private void SwitchWalletAddress()
        {
            //var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            //_dialogService.Show<WalletAddressSelectionModal>("Select Wallet Address", options);
        }

        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), "Are you sure you want to logout?"},
                {nameof(Dialogs.Logout.ButtonText), "Logout"},
                {nameof(Dialogs.Logout.Color), Color.Error},
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>("Logout", parameters, options);
        }
    }
}
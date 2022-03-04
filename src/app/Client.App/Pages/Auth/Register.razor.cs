using Application.Common.Dtos.Response;
using Application.CreatorPortal.Account.Commands.Login;
using Application.CreatorPortal.Account.Commands.Register;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Auth
{
    public partial class Register : IPageBase
    {
        [Inject] public IFaucetManager FaucetManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private RegisterCommand Model { get; set; } = new();
        public bool IsProcessing { get; set; }
        public bool IsGetWalletProcessing { get; set; }

        public FaucetResponseDto Wallet { get; set; }

        private async Task SubmitAsync()
        {
            if (Validated)
            {
                if (Wallet == null)
                {
                    _appDialogService.ShowError("Wallet is required. Use the 'GET XRP Wallet' button to get one.");
                    return;
                }

                IsProcessing = true;

                try
                {
                    Model.AccountXAddress = Wallet.Account.XAddress;
                    Model.AccountSecret = Wallet.Account.Secret;
                    Model.AccountClassicAddress = Wallet.Account.ClassicAddress;
                    Model.AccountAddress = Wallet.Account.Address;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.RegisterAsync(Model));
                    _appDialogService.ShowSuccess(string.Format("Hi {0}, you've successfully registered. Please login to continue.", Model.Username));
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/", true);
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                }

                IsProcessing = false;
            }
        }

        private async Task OnGetWalletAsync()
        {
            IsGetWalletProcessing = true;

            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => FaucetManager.GenerateAccountAsync());
                Wallet = result.Data;
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

            IsGetWalletProcessing = false;
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Model.Bio = string.Empty;
        }
    }
}
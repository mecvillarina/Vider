using Application.CreatorPortal.Account.Commands.Login;
using Blazored.FluentValidation;
using Client.Infrastructure.Exceptions;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Auth
{
    public partial class Login : IPageBase
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private LoginCommand Model { get; set; } = new();
        public bool IsProcessing { get; set; }

        private async Task SubmitAsync()
        {
            if (Validated)
            {
                IsProcessing = true;

                try
                {
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.LoginAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetProfileAsync());
                    _appDialogService.ShowSuccess(string.Format("Welcome back {0}", Model.Username));
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/profile", true);
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
    }
}
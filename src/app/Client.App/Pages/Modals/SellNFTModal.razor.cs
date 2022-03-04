using Application.CreatorPortal.NFTs.Commands.SellNFT;
using Application.CreatorPortal.NFTs.Dtos;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class SellNFTModal : IPageBase
    {
        [Inject] public INFTManager NFTManager { get; set; }
        [Parameter] public NFTItemDto NFT { get; set; }
        [Parameter] public SellNFTCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SellAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.SellNFTAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                    _appDialogService.ShowSuccess($"You've successfully created a sell offer for your NFT. You can check it out on marketplace.");
                    MudDialog.Close();
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
    }
}
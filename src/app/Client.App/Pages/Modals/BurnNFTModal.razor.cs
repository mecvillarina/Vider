using Application.CreatorPortal.NFTs.Commands.BurnNFT;
using Application.CreatorPortal.NFTs.Commands.ClaimNFT;
using Application.CreatorPortal.NFTs.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class BurnNFTModal : IPageBase
    {
        [Inject] public INFTManager NFTManager { get; set; }
        [Parameter] public NFTItemDto NFT { get; set; }
        [Parameter] public BurnNFTCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task BurnAsync()
        {
            try
            {
                IsProcessing = true;
                await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.BurnNFTAsync(Model));
                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                _appDialogService.ShowSuccess("You've successfully burned your NFT.");
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
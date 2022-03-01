using Application.CreatorPortal.NFTs.Commands.MintNFT;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class MintNFTModal : IPageBase
    {
        [Inject] public INFTManager NFTManager { get; set; }
        [Inject] public ICreatorManager CreatorManager { get; set; }

        [Parameter] public MintNFTCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public string PhotoLink { get; set; }
        public IBrowserFile SelectedFile { get; set; }
        public bool IsPhotoOverlayVisible { get; set; }
        public bool IsUploadingPhoto { get; set; }
        private const long _maxFileSize = 1024 * 1024 * 10;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task MintAsync()
        {
            if (Validated && !IsUploadingPhoto)
            {
                try
                {
                    if (SelectedFile == null)
                    {
                        _appDialogService.ShowError("Please select an image.");
                        return;
                    }

                    IsProcessing = true;

                    Model.Filename = PhotoLink.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

                    await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.MintNFTAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                    _appDialogService.ShowSuccess($"You've successfully minted an NFT.");
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

        private void TogglePhotoOvelay(bool visible)
        {
            IsPhotoOverlayVisible = visible;
        }

        private async Task OnPhotoChange(InputFileChangeEventArgs e)
        {

            foreach (var file in e.GetMultipleFiles(1))
            {
                SelectedFile = file;
                var stream = file.OpenReadStream(_maxFileSize);
                await UploadDumpPhotoAsync(file.Name, stream);
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task UploadDumpPhotoAsync(string filename, Stream stream)
        {
            try
            {
                IsUploadingPhoto = true;
                await InvokeAsync(StateHasChanged);

                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorManager.UploadDumpAsync(stream, filename));
                PhotoLink = result.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            IsUploadingPhoto = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}
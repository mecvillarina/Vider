using Application.CreatorPortal.CreatorRewards.Commands.Upload;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class AddRewardModal : IPageBase
    {
        [Inject] public ICreatorManager CreatorManager { get; set; }
        [Inject] public ICreatorRewardManager CreatorRewardManager { get; set; }

        [Parameter] public UploadRewardCommand Model { get; set; } = new();
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

        private async Task AddAsync()
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
                    var stream = SelectedFile.OpenReadStream(_maxFileSize);
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorRewardManager.UploadRewardAsync(Model, stream, SelectedFile.Name));
                    _appDialogService.ShowSuccess("NFT Reward for your new subscriber has been added.");
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
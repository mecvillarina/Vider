using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class DeletePostModal : IPageBase
    {
        [Inject] public IFeedManager FeedManager { get; set; }

        [Parameter] public DeleteFeedPostCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task DeleteAsync()
        {
            try
            {
                IsProcessing = true;
                await _exceptionHandler.HandlerRequestTaskAsync(() => FeedManager.DeletePostAsync(Model));
                _appDialogService.ShowSuccess($"Your post has been deleted.");
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
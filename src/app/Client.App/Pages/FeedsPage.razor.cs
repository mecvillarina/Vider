using Application.CreatorPortal.Feeds.Commands.LikePost;
using Application.CreatorPortal.Feeds.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Modals;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class FeedsPage : IAsyncDisposable
    {
        [Inject] public IFeedManager FeedManager { get; set; }
        [Inject] IBreakpointService BreakpointListener { get; set; }

        private Guid _breakpointSubscriptionId;
        private Breakpoint _currentBreakpoint;
        public string CardStyle { get; set; }

        public bool IsLoaded { get; set; }
        public List<FeedPostItemDto> Posts { get; set; } = new();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var subscriptionResult = await BreakpointListener.Subscribe((breakpoint) =>
                {
                    _currentBreakpoint = breakpoint;
                    SetStyles();
                    InvokeAsync(StateHasChanged);
                }, new ResizeOptions
                {
                    ReportRate = 250,
                    NotifyOnBreakpointOnly = true,
                });

                _currentBreakpoint = subscriptionResult.Breakpoint;
                _breakpointSubscriptionId = subscriptionResult.SubscriptionId;
                SetStyles();
                await FetchRecentPostsAsync();

                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        private void SetStyles()
        {
            if (_currentBreakpoint == Breakpoint.Xs)
            {
                CardStyle = "width: 100%";
            }
            else
            {
                CardStyle = "width: 600px";
            }
        }

        private async Task FetchRecentPostsAsync()
        {
            try
            {
                IsLoaded = false;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => FeedManager.GetRecentPostsAsync());
                Posts = result.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            finally
            {
                Posts ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeCreatePostModalAsync()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters() { };

            var dialog = _dialogService.Show<CreatePostModal>($"NEW POST", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchRecentPostsAsync();
            }
        }

        private async Task OnLikePostAsync(FeedPostItemDto item)
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => FeedManager.LikePostAsync(new LikeFeedPostCommand() { PostId = item.PostId }));
                item.IsLiked = result.Data.IsLiked;
                item.LikedCount = result.Data.Count;
                await InvokeAsync(StateHasChanged);
            }
            catch
            {

            }
        }

        private async Task InvokeViewCreatorProfileAsync(string creatorUsername)
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(ViewCreatorProfileModal.Username), creatorUsername }
            };

            var dialog = _dialogService.Show<ViewCreatorProfileModal>($"{creatorUsername.ToUpper()}'s Profile", parameters, options);
            await dialog.Result;
        }

        public async ValueTask DisposeAsync()
        {
            await BreakpointListener.Unsubscribe(_breakpointSubscriptionId);
        }
    }
}

using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using Application.CreatorPortal.Feeds.Dtos;
using Application.CreatorPortal.NFTs.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class ViewCreatorProfileModal
    {
        [Inject] private ICreatorManager CreatorManager { get; set; }
        [Inject] private ICreatorRewardManager CreatorRewardManager { get; set; }
        [Inject] private ICreatorSubscriberManager CreatorSubscriberManager { get; set; }
        [Inject] private INFTManager NFTManager { get; set; }
        [Inject] private IFeedManager FeedManager { get; set; }
        [Inject] IBreakpointService BreakpointListener { get; set; }

        [Parameter] public string Username { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private Guid _breakpointSubscriptionId;
        private Breakpoint _currentBreakpoint;
        public string NFTCardStyle { get; set; }
        public string FeedCardStyle { get; set; }
        public string FlexDirection { get; set; }

        public bool IsFetchingNFTs { get; set; }
        public bool IsFetchingFeeds { get; set; }

        public bool IsMe { get; set; }
        public CreatorDto Creator { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsSubscribing { get; set; }
        public List<GetRewardItemDto> Rewards { get; set; } = new();
        public List<NFTItemDto> NFTs { get; set; } = new();
        public List<FeedPostItemDto> Posts { get; set; } = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetBreakpointSubscriptionAsync();
                SetStyles();

                await FetchMyProfileAsync();
                await FetchCreatorAsync();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
                await FetchFeedPostsAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task SetBreakpointSubscriptionAsync()
        {
            var subscriptionResult = await BreakpointListener.Subscribe(async (breakpoint) =>
            {
                _currentBreakpoint = breakpoint;
                SetStyles();
                await InvokeAsync(StateHasChanged);
            }, new ResizeOptions
            {
                ReportRate = 250,
                NotifyOnBreakpointOnly = true,
            });

            _currentBreakpoint = subscriptionResult.Breakpoint;
            _breakpointSubscriptionId = subscriptionResult.SubscriptionId;
        }

        private void SetStyles()
        {

            if (_currentBreakpoint == Breakpoint.Xs)
            {
                NFTCardStyle = "";
                FeedCardStyle = "width:100%;";
                FlexDirection = "flex-column";
            }
            else
            {
                NFTCardStyle = "width:300px;";
                FeedCardStyle = "width:600px;";
                FlexDirection = "flex-wrap flex-row";
            }
        }

        private async Task FetchMyProfileAsync()
        {
            var profile = await _accountManager.FetchProfileAsync();
            if (profile == null)
            {
                MudDialog.Close();
                return;
            }

            IsMe = profile.Username == Username;
        }

        private async Task FetchCreatorAsync()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorManager.GetProfileAsync(Username));
                Creator = result.Data;
                var rewardResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorRewardManager.GetRewardsAsync(Creator.Id));
                Rewards = rewardResult.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
                MudDialog.Close();
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
                MudDialog.Close();
            }
        }

        private async Task FetchNFTsAsync()
        {
            try
            {
                if (IsFetchingNFTs) return;

                IsFetchingNFTs = true;
                await InvokeAsync(StateHasChanged);
                var creatorNFTResult = await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.GetCreatorNFTsAsync(Creator.Id));
                NFTs = creatorNFTResult.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                NFTs ??= new();
                IsFetchingNFTs = false;
                await InvokeAsync(StateHasChanged);
            }
        }


        private async Task FetchFeedPostsAsync()
        {
            try
            {
                if (IsFetchingFeeds) return;

                IsFetchingFeeds = true;
                await InvokeAsync(StateHasChanged);
                var feedPostsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => FeedManager.GetPostsAsync(Creator.Id));
                Posts = feedPostsResult.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                Posts ??= new();
                IsFetchingFeeds = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task Subscribe()
        {
            try
            {
                IsSubscribing = true;
                var subscribeResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorSubscriberManager.SubscribeAsync(new SubscribeCommand() { CreatorId = Creator.Id }));
                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetWalletAsync());
                var rewardsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorRewardManager.GetRewardsAsync(Creator.Id));

                if (rewardsResult.Data.Any())
                {
                    _appDialogService.ShowSuccess($"Subscription Added. Subscription NFT Reward from {Creator.Username} will arrive shortly (ETA: 1-2 mins) to your account and can be claim on the 'Claims/Gifts' tab of your Profile.");
                }
                else
                {
                    _appDialogService.ShowSuccess("Subscription Added.");
                }

                Creator.IsSubscribed = true;
                Creator.SubscriberCount = subscribeResult.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                IsSubscribing = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void OnTabPanelIndexChanged(int index)
        {
            switch (index)
            {
                case 0: await FetchFeedPostsAsync(); break;
                case 1: await FetchNFTsAsync(); break;
            }

            Console.WriteLine($"Tab: {index}");
        }
    }
}
using Application.CreatorPortal.Account.Queries.MyProfile;
using Application.CreatorPortal.Activities.Dtos;
using Application.CreatorPortal.CreatorRewards.Queries.GetRewards;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.Feeds.Commands.DeletePost;
using Application.CreatorPortal.Feeds.Dtos;
using Application.CreatorPortal.NFTs.Commands.BurnNFT;
using Application.CreatorPortal.NFTs.Commands.CancelSellNFT;
using Application.CreatorPortal.NFTs.Commands.ClaimNFT;
using Application.CreatorPortal.NFTs.Commands.GiftNFT;
using Application.CreatorPortal.NFTs.Commands.SellNFT;
using Application.CreatorPortal.NFTs.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Modals;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class ProfilePage : IAsyncDisposable
    {
        [Inject] public ICreatorRewardManager CreatorRewardManager { get; set; }
        [Inject] public INFTManager NFTManager { get; set; }
        [Inject] public IFeedManager FeedManager { get; set; }
        [Inject] public ICreatorSubscriberManager CreatorSubscriberManager { get; set; }

        [Inject] IBreakpointService BreakpointListener { get; set; }

        private MudTabs tabs { get; set; }
        private Guid _breakpointSubscriptionId;
        private Breakpoint _currentBreakpoint;
        public string CardStyle { get; set; }
        public string FeedCardStyle { get; set; }
        public string FlexDirection { get; set; }

        public MyProfileResponse Profile { get; set; }
        public string ProfilePictureLink { get; set; }
        public bool IsLoaded { get; set; }

        public bool IsFetchingRewards { get; set; }
        public bool IsFetchingNFTs { get; set; }
        public bool IsFetchingNFTClaims { get; set; }
        public bool IsFetchingFeeds { get; set; }
        public bool IsFetchingSubscribers { get; set; }
        public bool IsFetchingActivities { get; set; }

        public bool IsProfilePhotoOverlayVisible { get; set; }
        public bool IsUploadingProfilePhoto { get; set; }
        public List<GetRewardItemDto> Rewards { get; set; } = new();
        public List<NFTItemDto> NFTs { get; set; } = new();
        public List<NFTClaimDto> NFTClaims { get; set; } = new();
        public List<FeedPostItemDto> Posts { get; set; } = new();
        public List<SubscriberDto> Subscribers { get; set; } = new();
        public List<ActivityLogDto> Activities { get; set; } = new();
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetBreakpointSubscriptionAsync();
                SetStyles();

                await FetchProfileAsync();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
                await FetchRewardsAsync();
            }
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
                CardStyle = "";
                FeedCardStyle = "width:100%;";
                FlexDirection = "flex-column";
            }
            else
            {
                CardStyle = "width:300px;";
                FeedCardStyle = "width:600px;";
                FlexDirection = "flex-wrap flex-row";
            }
        }

        private async void OnTabPanelIndexChanged(int index)
        {
            switch (index)
            {
                case 0: await FetchRewardsAsync(); break;
                case 1: await FetchNFTsAsync(); break;
                case 2: await FetchNFTClaimsAsync(); break;
                case 3: await FetchSubscriberAsync(); break;
                case 4: await FetchFeedPostsAsync(); break;
                case 5: await FetchActivitiesAsync(); break;
            }
        }
        private async Task FetchProfileAsync()
        {
            Profile = await _accountManager.FetchProfileAsync();
        }

        private void ToggleProfilePhotoOvelay(bool visible)
        {
            IsProfilePhotoOverlayVisible = visible;
        }

        private async Task FetchRewardsAsync()
        {
            try
            {
                if (IsFetchingRewards) return;

                IsFetchingRewards = true;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorRewardManager.GetRewardsAsync());
                Rewards = result.Data;
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
                Rewards ??= new();
                IsFetchingRewards = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task FetchNFTsAsync()
        {
            try
            {
                if (IsFetchingNFTs) return;

                IsFetchingNFTs = true;
                await InvokeAsync(StateHasChanged);
                var creatorNFTResult = await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.GetCreatorNFTsAsync());
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

        private async Task FetchNFTClaimsAsync()
        {
            try
            {
                if (IsFetchingNFTClaims) return;

                IsFetchingNFTClaims = true;
                await InvokeAsync(StateHasChanged);
                var nftClaimsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.GetNFTClaimsAsync());
                NFTClaims = nftClaimsResult.Data;
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
                NFTClaims ??= new();
                IsFetchingNFTClaims = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task FetchSubscriberAsync()
        {
            try
            {
                if (IsFetchingSubscribers) return;

                IsFetchingSubscribers = true;
                await InvokeAsync(StateHasChanged);
                var subscribersResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorSubscriberManager.GetSubscribersAsync());
                Subscribers = subscribersResult.Data;
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
                Subscribers ??= new();
                IsFetchingSubscribers = false;
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
                var feedPostsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => FeedManager.GetPostsAsync());
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

        private async Task FetchActivitiesAsync()
        {
            try
            {
                if (IsFetchingActivities) return;

                IsFetchingActivities = true;
                await InvokeAsync(StateHasChanged);
                var activitiesResult = await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetRecentActivitiesAsync());
                Activities = activitiesResult.Data;
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
                Activities ??= new();
                IsFetchingActivities = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        private async Task OnProfilePictureChange(InputFileChangeEventArgs e)
        {
            long maxFileSize = 1024 * 1024 * 10;

            foreach (var file in e.GetMultipleFiles(1))
            {
                var stream = file.OpenReadStream(maxFileSize);
                await UpdateProfilePhotoAsync(file.Name, stream);
            }
        }

        private async Task UpdateProfilePhotoAsync(string filename, Stream stream)
        {
            try
            {
                IsUploadingProfilePhoto = true;
                await InvokeAsync(StateHasChanged);

                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.UploadPhotoAsync(stream, filename));
                await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.GetProfileAsync());
                await FetchProfileAsync();
                _appDialogService.ShowSuccess("Profile Photo Updated.");
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            IsUploadingProfilePhoto = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task InvokeAddRewardModalAsync()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters() { };

            var dialog = _dialogService.Show<AddRewardModal>($"ADD NFT REWARD FOR NEW SUBSCRIBER", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchRewardsAsync();
            }
        }

        private async Task InvokeClaimNFTModalAsync(NFTClaimDto claim)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(ClaimNFTModal.Claim), claim },
                { nameof(ClaimNFTModal.Model), new ClaimNFTCommand() { Id = claim.Id, TokenId = claim.TokenId } },
            };

            var dialog = _dialogService.Show<ClaimNFTModal>($"CLAIM AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTClaimsAsync();
            }
        }

        private async Task InvokeBurnNFTModalAsync(NFTItemDto nft)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(BurnNFTModal.NFT), nft },
                { nameof(BurnNFTModal.Model), new BurnNFTCommand() { Id = nft.Id, TokenId = nft.TokenId } },
            };

            var dialog = _dialogService.Show<BurnNFTModal>($"BURN AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTsAsync();
            }
        }

        private async Task InvokeGiftNFTModalAsync(NFTItemDto nft)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(GiftNFTModal.NFT), nft },
                { nameof(GiftNFTModal.Model), new GiftNFTCommand() { Id = nft.Id, TokenId = nft.TokenId } },
            };

            var dialog = _dialogService.Show<GiftNFTModal>($"GIFT AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTsAsync();
            }
        }

        private async Task InvokeMintNFTModalAsync()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters() { };

            var dialog = _dialogService.Show<MintNFTModal>($"CREATE/MINT AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTsAsync();
            }
        }

        private async Task InvokeSellNFTModalAsync(NFTItemDto nft)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(SellNFTModal.NFT), nft },
                { nameof(SellNFTModal.Model), new SellNFTCommand() { Id = nft.Id, TokenId = nft.TokenId } },
            };

            var dialog = _dialogService.Show<SellNFTModal>($"SELL AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTsAsync();
            }
        }

        private async Task InvokeCancelSellNFTModalAsync(NFTItemDto nft)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(CancelSellNFTModal.NFT), nft },
                { nameof(CancelSellNFTModal.Model), new CancelSellNFTCommand() { Id = nft.Id, TokenId = nft.TokenId } },
            };

            var dialog = _dialogService.Show<CancelSellNFTModal>($"CANCEL NFT SELL", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchNFTsAsync();
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
                await FetchNFTsAsync();
            }
        }

        private async Task InvokeDeletePostModalAsync(int id)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(DeletePostModal.Model), new DeleteFeedPostCommand() { Id = id  } }
            };

            var dialog = _dialogService.Show<DeletePostModal>($"DELETE POST CONFIRMATION", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchFeedPostsAsync();
            }
        }

        private async Task InvokeViewTxModalAsync(string txHash)
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.False, FullWidth = true, DisableBackdropClick = false };
            var parameters = new DialogParameters()
            {
                { nameof(ViewTxModal.TxHash), txHash }
            };

            var dialog = _dialogService.Show<ViewTxModal>($"VIEW XRPL TX - {txHash}", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchFeedPostsAsync();
            }
        }


        public async ValueTask DisposeAsync()
        {
            await BreakpointListener.Unsubscribe(_breakpointSubscriptionId);
        }
    }
}

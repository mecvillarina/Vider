using Application.CreatorPortal.NFTs.Commands.BuyNFT;
using Application.CreatorPortal.NFTs.Dtos;
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
    public partial class MarketplacePage : IAsyncDisposable
    {
        [Inject] public INFTManager NFTManager { get; set; }
        [Inject] IBreakpointService BreakpointListener { get; set; }

        private Guid _breakpointSubscriptionId;
        private Breakpoint _currentBreakpoint;
        public string CardStyle { get; set; } = string.Empty;
        public string FlexDirection { get; set; } = string.Empty;

        public bool IsLoaded { get; set; }
        public List<NFTSellOfferItemDto> SellOffers { get; set; } = new();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
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
                SetStyles();
                await FetchSellOffersAsync();

                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        private void SetStyles()
        {

            if (_currentBreakpoint == Breakpoint.Xs)
            {
                CardStyle = "";
                FlexDirection = "flex-column";
            }
            else
            {
                CardStyle = "width: 300px;";
                FlexDirection = "flex-wrap flex-row";
            }
        }

        private async Task FetchSellOffersAsync(string query = "")
        {
            try
            {
                IsLoaded = false;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.GetNFTSellOffersAsync(query));
                SellOffers = result.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            finally
            {
                SellOffers ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeBuyNFTModalAsync(NFTSellOfferItemDto sellOffer)
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(BuyNFTModal.SellOffer), sellOffer },
                { nameof(BuyNFTModal.Model), new BuyNFTCommand() { Id = sellOffer.SellOfferId, TokenId = sellOffer.NFT.TokenId } },
            };

            var dialog = _dialogService.Show<BuyNFTModal>($"BUY AN NFT", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchSellOffersAsync();
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

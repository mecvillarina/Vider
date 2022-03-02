using Application.CreatorPortal.Creators.Dtos;
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
    public partial class CreatorsPage : IAsyncDisposable
    {
        [Inject] public ICreatorManager CreatorManager { get; set; }
        [Inject] IBreakpointService BreakpointListener { get; set; }

        private Guid _breakpointSubscriptionId;
        private Breakpoint _currentBreakpoint;
        public string CreatorCardStyle { get; set; } = string.Empty;
        public string FlexDirection { get; set; } = string.Empty;

        public List<CreatorDto> Creators { get; set; } = new();
        public bool IsLoaded { get; set; }

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
                await FetchCreatorsAsync();
                _ = await Task.FromResult(0);
            }
        }

        private void SetStyles()
        {

            if (_currentBreakpoint == Breakpoint.Xs)
            {
                CreatorCardStyle = "";
                FlexDirection = "flex-column";
            }
            else
            {
                CreatorCardStyle = "width:300px;";
                FlexDirection = "flex-wrap flex-row";
            }
        }
        private async Task FetchCreatorsAsync(string query = "")
        {
            try
            {
                IsLoaded = false;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CreatorManager.SearchAsync(query, 25));
                Creators = result.Data;
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
                Creators ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeViewCreatorProfileAsync(CreatorDto creator)
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters()
            {
                { nameof(ViewCreatorProfileModal.Username), creator.Username }
            };

            var dialog = _dialogService.Show<ViewCreatorProfileModal>($"{creator.Username.ToUpper()}'s Profile", parameters, options);
            await dialog.Result;
        }

        public async ValueTask DisposeAsync()
        {
            await BreakpointListener.Unsubscribe(_breakpointSubscriptionId);
        }

    }
}

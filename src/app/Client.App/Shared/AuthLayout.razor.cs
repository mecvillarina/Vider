using Client.Infrastructure.Settings;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client.App.Shared
{
    public partial class AuthLayout
    {
        [Inject] IBreakpointService BreakpointListener { get; set; }
        public string HeightStyle { get; set; }
        public MudTheme CurrentTheme { get; set; }
        
        private Guid _subscriptionId;
        private Breakpoint _currentBreakpoint;

        protected override void OnInitialized()
        {
            CurrentTheme = _clientPreferenceManager.GetCurrentTheme();
        }

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
                _subscriptionId = subscriptionResult.SubscriptionId;
                SetStyles();
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void SetStyles()
        {
            if (_currentBreakpoint == Breakpoint.Xs)
            {
                HeightStyle = "100%";
            }
            else
            {
                HeightStyle = "height: 100vh;";
            }
        }

        public async ValueTask DisposeAsync() => await BreakpointListener.Unsubscribe(_subscriptionId);

    }
}

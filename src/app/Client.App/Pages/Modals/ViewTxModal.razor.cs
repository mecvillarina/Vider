using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Modals
{
    public partial class ViewTxModal
    {
        [Inject] public INFTManager NFTManager { get; set; }
        [Parameter] public string TxHash { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsLoaded { get; set; }

        public string Display { get; set; }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    var result = await _exceptionHandler.HandlerRequestTaskAsync(() => NFTManager.GetTxAsync(TxHash));
                    Display = result.Data;
                    IsLoaded = true;
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

                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
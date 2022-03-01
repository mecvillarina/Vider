using Application.CreatorPortal.Account.Queries.GetWallet;
using Client.App.Infrastructure.Managers;
using System;
using System.Timers;

namespace Client.App.PeriodicExecutors
{
    public class RenderWalletExecutor : IDisposable
    {
        private readonly IAccountManager _accountManager;

        private Timer _timer;
        private bool _running;

        public RenderWalletExecutor(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public event EventHandler<GetWalletResponse> JobExecuted;

        public void StartExecuting()
        {
            if (!_running)
            {
                _timer = new Timer();
                _timer.Interval = 3000;
                _timer.Elapsed += HandleTimer;
                _timer.AutoReset = true;
                _timer.Enabled = true;
                _running = true;
            }
        }

        async void HandleTimer(object source, ElapsedEventArgs e)
        {
            try
            {
                var wallet = await _accountManager.FetchWalletAsync();

                if (wallet != null)
                {
                    JobExecuted?.Invoke(this, wallet);
                }
            }
            catch
            {
                Console.WriteLine($"Fetch Wallet Executor: Fetch Error");
            }
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}

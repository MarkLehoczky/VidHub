using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Logics;

namespace VidHub.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly ILogger logger = VidHubContext.Logger;
        private bool closingConfirmed = false;

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            try
            {
                logger.LogTrace("MainWindow initializing: loading settings");
                VidHubSettings.Instance.Load();
                logger.LogInformation("Settings loaded on MainWindow init");
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Failed to load settings during MainWindow initialization");
            }
            
            
            Closed += async (o, e) =>
            {
                if (!VidHubContext.Host.GetService<IVideoLoadService>().HasActiveTransfer || closingConfirmed)
                {
                    return;
                }

                e.Handled = true;
                var exit = await VidHubContext.Window.OpenCloseInterruptedDialog();
                if (exit)
                {
                    closingConfirmed = true;
                    Close();
                }
            };
        }
    }
}
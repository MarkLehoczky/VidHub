using Microsoft.UI.Xaml;
using VidHub.Core.Settings;
using Microsoft.Extensions.Logging;

namespace VidHub.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly ILogger logger = global::VidHub.Platform.VidHubEnvironment.VidHubContext.Logger;

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
        }
    }
}

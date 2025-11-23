using Microsoft.UI.Xaml;
using VidHub.Core.Settings;

namespace VidHub.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            Platform.Context.Host.GetService<ISettingsService>().Load();
        }
    }
}

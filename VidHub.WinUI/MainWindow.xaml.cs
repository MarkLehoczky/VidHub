using Microsoft.UI.Xaml;
using VidHub.Services.Settings.Interfaces;

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

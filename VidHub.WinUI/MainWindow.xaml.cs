using Microsoft.UI.Xaml;
using VidHub.Platform;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            Context.MainHost.GetService<ISettingsService>().Load();
            Context.MainHost.GetService<IVideoOrganizeService>().Load();
        }
    }
}

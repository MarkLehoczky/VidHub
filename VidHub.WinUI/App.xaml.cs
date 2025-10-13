using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using VidHub.Services.Base;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Modals;
using VidHub.Services.Modals.Interfaces;
using VidHub.Services.Settings;
using VidHub.Services.Settings.Interfaces;
using VidHub.Services.System;
using VidHub.Services.System.Interfaces;
using VidHub.ViewModels;
using VidHub.ViewModels.Modals;
using VidHub.WinUI.Context;

namespace VidHub.WinUI
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            Platform.Context.Host = new HostContext(Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IVideoService, VideoService>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<ISystemManager, SystemManager>();
                    services.AddSingleton<IVideoLoadService, VideoLoadService>();
                    services.AddSingleton<IVideoOrganizerService, VideoOrganizerService>();
                    services.AddSingleton<IVideoCollectionService, VideoCollectionService>();
                    services.AddSingleton<IVideoTitleFormatCustomizationService, VideoTitleFormatCustomizationService>();
                    services.AddSingleton<IVideoPreviewImageCustomizationService, VideoPreviewImageCustomizationService>();
                    services.AddTransient<TitleBarViewModel>();
                    services.AddTransient<SidePanelViewModel>();
                    services.AddTransient<VideoCollectionViewModel>();
                    services.AddTransient<VideoTitleFormatCustomizationViewModel>();
                    services.AddTransient<VideoPreviewImageCustomizationViewModel>();
                })
                .Build());
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            Platform.Context.Window = new WindowContext(_window);
            _window.Activated += (s, e) => Platform.Context.Window.IsActive = e.WindowActivationState != WindowActivationState.Deactivated;
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Platform.Context.Host.GetService<ISettingsService>().Save();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using VidHub.Services.Base;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Connectors.Modals;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.Services.Logics;
using VidHub.Services.Logics.Interfaces;
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
                    services.AddSingleton<IVideoCollectionConnector, VideoCollectionConnector>();
                    services.AddSingleton<ISidePanelConnector, SidePanelConnector>();
                    services.AddSingleton<ITitleBarConnector, TitleBarConnector>();
                    services.AddSingleton<IVideoDisplayCustomizationConnector, VideoDisplayCustomizationConnector>();
                    services.AddSingleton<IVideoTitleFormatCustomizationConnector, VideoTitleFormatCustomizationConnector>();
                    services.AddSingleton<IVideoPreviewImageCustomizationConnector, VideoPreviewImageCustomizationConnector>();
                    services.AddTransient<TitleBarViewModel>();
                    services.AddTransient<SidePanelViewModel>();
                    services.AddTransient<VideoCollectionViewModel>();
                    services.AddTransient<VideoDisplayCustomizationViewModel>();
                    services.AddTransient<VideoTitleFormatCustomizationViewModel>();
                    services.AddTransient<VideoPreviewImageCustomizationViewModel>();
                    services.AddTransient<RenameViewModel>();
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

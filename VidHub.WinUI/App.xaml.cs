using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using VidHub.Core.Settings;
using VidHub.Services.Base;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Base;
using VidHub.Services.Connectors.Base.Interfaces;
using VidHub.Services.Connectors.Modals;
using VidHub.Services.Connectors.Modals.Interfaces;
using VidHub.Services.Logics;
using VidHub.Services.Logics.Interfaces;
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
                    _ = services.AddSingleton<IVideoService, VideoService>();
                    _ = services.AddSingleton<IVidHubSettings, VidHubSettings>();
                    _ = services.AddSingleton<ISystemManager, SystemManager>();
                    _ = services.AddSingleton<IVideoLoadService, VideoLoadService>();
                    _ = services.AddSingleton<IVideoOrganizerService, VideoOrganizerService>();
                    _ = services.AddSingleton<IVideoCollectionService, VideoCollectionService>();
                    _ = services.AddSingleton<IVideoCollectionConnector, VideoCollectionConnector>();
                    _ = services.AddSingleton<ISidePanelConnector, SidePanelConnector>();
                    _ = services.AddSingleton<ITitleBarConnector, TitleBarConnector>();
                    _ = services.AddSingleton<IVideoDisplayCustomizationConnector, VideoDisplayCustomizationConnector>();
                    _ = services.AddSingleton<IVideoTitleFormatCustomizationConnector, VideoTitleFormatCustomizationConnector>();
                    _ = services.AddSingleton<IVideoPreviewImageCustomizationConnector, VideoPreviewImageCustomizationConnector>();
                    _ = services.AddTransient<TitleBarViewModel>();
                    _ = services.AddTransient<SidePanelViewModel>();
                    _ = services.AddTransient<VideoCollectionViewModel>();
                    _ = services.AddTransient<VideoDisplayCustomizationViewModel>();
                    _ = services.AddTransient<VideoTitleFormatCustomizationViewModel>();
                    _ = services.AddTransient<VideoPreviewImageCustomizationViewModel>();
                    _ = services.AddTransient<RenameViewModel>();
                })
                .Build());
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            Platform.Context.Window = new WindowContext(_window);
            _window.Activated += (s, e) => Platform.Context.Window.IsActive = e.WindowActivationState != WindowActivationState.Deactivated;
            AppDomain.CurrentDomain.ProcessExit += (_, _) => VidHubSettings.Instance.Save();
        }
    }
}

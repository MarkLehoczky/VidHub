using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.Connectors.Base;
using VidHub.Services.Connectors.Dialogs;
using VidHub.Services.Logics;
using VidHub.Services.System;
using VidHub.ViewModels;
using VidHub.ViewModels.Dialogs;
using VidHub.WinUI.Context;

namespace VidHub.WinUI
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            VidHubContext.Host = new HostContext(Host.CreateDefaultBuilder()
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
                    _ = services.AddSingleton<IDisplayFormatConnector, DisplayFormatConnector>();
                    _ = services.AddSingleton<ITitleFormatConnector, PassiveTitleFormatConnector>();
                    _ = services.AddSingleton<IPreviewImageFormatConnector, PreviewImageFormatConnector>();
                    _ = services.AddSingleton<ITagConnector, TagConnector>();
                    _ = services.AddTransient<TitleBarViewModel>();
                    _ = services.AddTransient<SidePanelViewModel>();
                    _ = services.AddTransient<VideoCollectionViewModel>();
                    _ = services.AddTransient<DisplayFormatViewModel>();
                    _ = services.AddTransient<TitleFormatViewModel>();
                    _ = services.AddTransient<PreviewImageFormatViewModel>();
                    _ = services.AddTransient<RenameViewModel>();
                })
                .Build());
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            VidHubContext.Window = new WindowContext(_window);
            _window.Activated += (s, e) => VidHubContext.Window.IsActive = e.WindowActivationState != WindowActivationState.Deactivated;
            _window.Closed += (s, e) => VidHubSettings.Instance.Save();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform;
using VidHub.Platform.Interfaces;
using VidHub.Services.Base;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings;
using VidHub.Services.Settings.Interfaces;
using VidHub.Services.System;
using VidHub.Services.System.Interfaces;
using VidHub.ViewModels;
using VidHub.ViewModels.Modals;
using VidHub.WinUI.UserControls.Modals;
using WinRT.Interop;

namespace VidHub.WinUI
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            Context.MainHost = new HostContext(Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IMainService, MainService>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<ISystemManager, SystemManager>();
                    services.AddSingleton<IVideoLoadService, VideoLoadService>();
                    services.AddSingleton<IVideoOrganizeService, VideoOrganizeService>();
                    services.AddSingleton<IVideoCollectionService, VideoCollectionService>();
                    services.AddSingleton<IVideoCustomizationService, VideoCustomizationService>();
                    services.AddTransient<TitlebarViewModel>();
                    services.AddTransient<SidepanelViewModel>();
                    services.AddTransient<VideoCollectionViewModel>();
                    services.AddTransient<TitleCustomizationViewModel>();
                    services.AddTransient<VideoCustomizationViewModel>();
                })
                .Build());
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            Context.MainWindow = new WindowContext(_window);
            _window.Activated += (s, e) => Context.MainWindow.IsActive = e.WindowActivationState != WindowActivationState.Deactivated;
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Context.MainHost.GetService<IVideoOrganizeService>().Save();
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Context.MainHost.GetService<ISettingsService>().Save();
        }
    }



    public class WindowContext(Window window) : IWindowContext
    {
        public object Window => window;

        public nint HWND => WindowNative.GetWindowHandle(window);

        public bool IsActive { get; set; }


        public bool TryEnqueue(Action callback)
        {
            return window.DispatcherQueue.TryEnqueue(callback.Invoke);
        }

        public async Task ShowDialogAsync(object type, string title, string closeButton)
        {
            object content = new();

            switch (type)
            {
                case ModalType.CustomizeDisplaying: content = new VideoCustomizationUserControl(); break;
                case ModalType.CustomizeLoading: content = new TitleCustomizationUserControl(); break;
            }

            var dialog = new ContentDialog()
            {
                Title = title,
                CloseButtonText = closeButton,
                DefaultButton = ContentDialogButton.Close,
                Content = content,
                XamlRoot = window.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        public async Task ShowDialogAsync(object type, string title, string closeButton, object instance)
        {
            object content = new();

            switch (type)
            {
                case ModalType.CustomizeDisplaying: content = new VideoCustomizationUserControl(); break;
                case ModalType.CustomizeLoading: content = new TitleCustomizationUserControl(); break;
                case ModalType.RenameVideo: content = new RenameUserControl((Video)instance); break;
            }

            var dialog = new ContentDialog()
            {
                Title = title,
                CloseButtonText = closeButton,
                DefaultButton = ContentDialogButton.Close,
                Content = content,
                XamlRoot = window.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }

    public class HostContext(IHost host) : IHostContext
    {
        public object Host => host;

        public T GetService<T>() where T : class
        {
            return host.Services.GetRequiredService<T>();
        }
    }
}

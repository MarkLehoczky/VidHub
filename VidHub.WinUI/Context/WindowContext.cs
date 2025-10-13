using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Platform.Interfaces;
using VidHub.WinUI.UserControls.Modals;
using WinRT.Interop;

namespace VidHub.WinUI.Context
{
    public class WindowContext(Window window) : IWindowContext
    {
        public nint HWND => WindowNative.GetWindowHandle(window);
        public bool IsActive { get; set; }


        public bool TryEnqueue(Action callback)
        {
            return window.DispatcherQueue.TryEnqueue(callback.Invoke);
        }

        public async Task ShowDialogAsync(ModalType type, string title, string closeButton)
        {
            object content = new();

            switch (type)
            {
                case ModalType.CustomizeVideoDisplayFormat: content = new VideoDisplayCustomizationUserControl(); break;
                case ModalType.CustomizeTitleFormat: content = new VideoTitleFormatCustomizationUserControl(); break;
                case ModalType.CustomizePreviewImageFrame: content = new VideoPreviewImageCustomizationUserControl(); break;
                case ModalType.ChangeVideoTitle: content = new RenameUserControl(new Video()); break;
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
        public async Task ShowDialogAsync(ModalType type, string title, string closeButton, object instance)
        {
            object content = new();

            switch (type)
            {
                case ModalType.CustomizeVideoDisplayFormat: content = new VideoDisplayCustomizationUserControl(); break;
                case ModalType.CustomizeTitleFormat: content = new VideoTitleFormatCustomizationUserControl(); break;
                case ModalType.CustomizePreviewImageFrame: content = new VideoPreviewImageCustomizationUserControl(); break;
                case ModalType.ChangeVideoTitle: content = new RenameUserControl((Video)instance); break;
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
}

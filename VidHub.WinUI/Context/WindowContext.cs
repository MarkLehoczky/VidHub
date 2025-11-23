using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VidHub.Core;
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

        public async Task ShowDialogAsync(string type, string title, string closeButton)
        {
            object content = new();

            switch (type)
            {
                case "CustomizeVideoDisplayFormat": content = new VideoDisplayCustomizationUserControl(); break;
                case "CustomizeTitleFormat": content = new VideoTitleFormatCustomizationUserControl(new Tuple<bool, IEnumerable<int>>(true, [])); break;
                case "CustomizePreviewImageFrame": content = new VideoPreviewImageCustomizationUserControl(); break;
                case "ChangeVideoTitle": content = new RenameUserControl(new Video()); break;
            }

            ContentDialog dialog = new()
            {
                Title = title,
                CloseButtonText = closeButton,
                DefaultButton = ContentDialogButton.Close,
                Content = content,
                XamlRoot = window.Content.XamlRoot
            };

            _ = await dialog.ShowAsync();
        }
        public async Task ShowDialogAsync(string type, string title, string closeButton, object instance)
        {
            object content = new();

            switch (type)
            {
                case "CustomizeVideoDisplayFormat": content = new VideoDisplayCustomizationUserControl(); break;
                case "CustomizeTitleFormat": content = new VideoTitleFormatCustomizationUserControl((Tuple<bool, IEnumerable<int>>)instance); break;
                case "CustomizePreviewImageFrame": content = new VideoPreviewImageCustomizationUserControl(); break;
                case "ChangeVideoTitle": content = new RenameUserControl((Video)instance); break;
            }

            ContentDialog dialog = new()
            {
                Title = title,
                CloseButtonText = closeButton,
                DefaultButton = ContentDialogButton.Close,
                Content = content,
                XamlRoot = window.Content.XamlRoot
            };

            _ = await dialog.ShowAsync();
        }
    }
}

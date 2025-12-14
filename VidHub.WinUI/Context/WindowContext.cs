using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
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
            try
            {
                callback ??= () => { };
                return window is Window actualWindow && actualWindow.DispatcherQueue != null && window.DispatcherQueue.TryEnqueue(callback.Invoke);
            }
            catch { return false; }
        }

        public async Task OpenDisplayFormatModal()
        {
            _ = TryEnqueue(async () =>
            {
                VideoDisplayCustomizationUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "FORMAT VIDEO COLLECTION",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenLicenseModal()
        {
            _ = TryEnqueue(async () =>
            {
                LicensesUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "COPYRIGHT",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenPreviewImageFormatModal()
        {
            _ = TryEnqueue(async () =>
            {
                VideoPreviewImageCustomizationUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "FORMAT PREVIEW IMAGES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenTitleFutureFormatModal()
        {
            _ = TryEnqueue(async () =>
            {
                VideoTitleFormatCustomizationUserControl content = new(false);
                ContentDialog dialog = new()
                {
                    Title = "FORMAT TITLES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenTitleFormatModal()
        {
            _ = TryEnqueue(async () =>
            {
                VideoTitleFormatCustomizationUserControl content = new(true);
                ContentDialog dialog = new()
                {
                    Title = "FORMAT TITLES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenVersionModal()
        {
            _ = TryEnqueue(async () =>
            {
                VersionsUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "VERSION CHANGES",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenVideoRenameModal(object obj)
        {
            _ = TryEnqueue(async () =>
            {
                if (obj == null || obj is not Video video)
                {
                    return;
                }
                RenameUserControl content = new(video);
                ContentDialog dialog = new()
                {
                    Title = $"Rename '{video.Title}'",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                _ = await dialog.ShowAsync();
            });
        }
    }
}

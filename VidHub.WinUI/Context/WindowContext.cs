using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Platform.Environment;
using VidHub.Services.Base;
using VidHub.WinUI.UserControls.Dialogs;
using WinRT.Interop;

namespace VidHub.WinUI.Context
{
    public class WindowContext(Window window) : IWindowContext
    {
        private bool hasOpenDialog = false;

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

        public async Task OpenDisplayFormatDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

            _ = TryEnqueue(async () =>
            {
                DisplayFormatUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "FORMAT VIDEO COLLECTION",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenLicensesDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

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
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenPreviewImageFormatDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

            _ = TryEnqueue(async () =>
            {
                PreviewImageFormatUserControl content = new();
                ContentDialog dialog = new()
                {
                    Title = "FORMAT PREVIEW IMAGES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenPassiveTitleFormatDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

            _ = TryEnqueue(async () =>
            {
                TitleFormatUserControl content = new(false);
                ContentDialog dialog = new()
                {
                    Title = "FORMAT TITLES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenActiveTitleFormatDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

            _ = TryEnqueue(async () =>
            {
                TitleFormatUserControl content = new(true);
                ContentDialog dialog = new()
                {
                    Title = "FORMAT TITLES",
                    CloseButtonText = "Finish",
                    DefaultButton = ContentDialogButton.Close,
                    Content = content,
                    XamlRoot = window.Content.XamlRoot
                };
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                dialog.CloseButtonClick += (_, _) =>
                {
                    foreach (Video video in Platform.Environment.Context.Host.GetService<IVideoService>())
                    {
                        video.LoadingFinished = true;
                    }
                };
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenVersionsDialog()
        {
            if (hasOpenDialog)
            {
                return;
            }

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
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }

        public async Task OpenRenameDialog(object obj)
        {
            if (hasOpenDialog)
            {
                return;
            }

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
                hasOpenDialog = true;
                dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                _ = await dialog.ShowAsync();
            });
        }
    }
}

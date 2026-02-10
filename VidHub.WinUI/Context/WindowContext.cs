using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.WinUI.UserControls.Dialogs;
using WinRT.Interop;

namespace VidHub.WinUI.Context
{
    public class WindowContext(Window window) : IWindowContext
    {
        private bool hasOpenDialog = false;
        private readonly ILogger logger = global::VidHub.Platform.VidHubEnvironment.VidHubContext.Logger;

        public nint HWND => WindowNative.GetWindowHandle(window);
        public bool IsActive { get; set; }


        public bool TryEnqueue(Action callback)
        {
            try
            {
                callback ??= () => { };
                bool result = window is Window actualWindow && actualWindow.DispatcherQueue != null && window.DispatcherQueue.TryEnqueue(callback.Invoke);
                logger.LogTrace("TryEnqueue result={Result}", result);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "TryEnqueue failed");
                return false;
            }
        }

        public async Task OpenDisplayFormatDialog()
        {
            logger.LogTrace("OpenDisplayFormatDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                    logger.LogInformation("DisplayFormat dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show DisplayFormat dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenLicensesDialog()
        {
            logger.LogTrace("OpenLicensesDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                    logger.LogInformation("Licenses dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Licenses dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenPreviewImageFormatDialog()
        {
            logger.LogTrace("OpenPreviewImageFormatDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                    logger.LogInformation("PreviewImageFormat dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show PreviewImageFormat dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenPassiveTitleFormatDialog()
        {
            logger.LogTrace("OpenPassiveTitleFormatDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                    logger.LogInformation("Passive TitleFormat dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Passive TitleFormat dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenActiveTitleFormatDialog()
        {
            logger.LogTrace("OpenActiveTitleFormatDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                        try
                        {
                            foreach (Video video in global::VidHub.Platform.VidHubEnvironment.VidHubContext.Host.GetService<IVideoService>())
                            {
                                video.LoadingFinished = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Failed to mark videos as LoadingFinished after ActiveTitleFormat dialog");
                        }
                    };
                    _ = await dialog.ShowAsync();
                    logger.LogInformation("Active TitleFormat dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Active TitleFormat dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenVersionsDialog()
        {
            logger.LogTrace("OpenVersionsDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
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
                    logger.LogInformation("Versions dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Versions dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenTagsDialog()
        {
            logger.LogTrace("OpenTagsDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
                {
                    TagsUserControl content = new();
                    ContentDialog dialog = new()
                    {
                        Title = "VIDEO TAGS",
                        CloseButtonText = "Finish",
                        DefaultButton = ContentDialogButton.Close,
                        Content = content,
                        XamlRoot = window.Content.XamlRoot
                    };
                    hasOpenDialog = true;
                    dialog.CloseButtonClick += (_, _) => hasOpenDialog = false;
                    _ = await dialog.ShowAsync();
                    logger.LogInformation("Tags dialog shown");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Tags dialog");
                    hasOpenDialog = false;
                }
            });
        }

        public async Task OpenRenameDialog(object obj)
        {
            logger.LogTrace("OpenRenameDialog requested, hasOpenDialog={Has}", hasOpenDialog);
            if (hasOpenDialog)
            {
                logger.LogDebug("Dialog already open, skipping");
                return;
            }

            _ = TryEnqueue(async () =>
            {
                try
                {
                    if (obj == null || obj is not Video video)
                    {
                        logger.LogWarning("OpenRenameDialog called with invalid object");
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
                    logger.LogInformation("Rename dialog shown for {File}", video.FilePath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to show Rename dialog");
                    hasOpenDialog = false;
                }
            });
        }
    }
}

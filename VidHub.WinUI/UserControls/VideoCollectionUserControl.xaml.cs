using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Logics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;

namespace VidHub.WinUI.UserControls
{
    public sealed partial class VideoCollectionUserControl : UserControl
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public VideoCollectionUserControl()
        {
            InitializeComponent();

            KeyboardAccelerator pasteAccelerator = new()
            {
                Key = Windows.System.VirtualKey.V,
                Modifiers = Windows.System.VirtualKeyModifiers.Control
            };
            pasteAccelerator.Invoked += PasteAccelerator_Invoked;

            KeyboardAccelerators.Add(pasteAccelerator);
            KeyboardAcceleratorPlacementMode = KeyboardAcceleratorPlacementMode.Hidden;
            logger.LogTrace("VideoCollectionUserControl initialized and keyboard accelerator registered");
        }


        private async void PasteAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;

            try
            {
                DataPackageView dataPackageView = Clipboard.GetContent();

                if (dataPackageView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await dataPackageView.GetStorageItemsAsync();
                    logger.LogDebug("Paste invoked with {Count} items", items.Count);
                    HandlePastedFiles(items);
                }
                else
                {
                    logger.LogTrace("Paste invoked but no storage items present");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "PasteAccelerator handling failed");
            }
        }

        private void DragOverItems(object sender, DragEventArgs e)
        {
            try
            {
                e.AcceptedOperation = e.DataView.Contains(StandardDataFormats.StorageItems) ? DataPackageOperation.Copy : DataPackageOperation.None;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "DragOverItems encountered exception");
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private async void DropItems(object sender, DragEventArgs e)
        {
            try
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync().AsTask();
                    logger.LogDebug("DropItems received {Count} items", items.Count);
                    HandlePastedFiles(items);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "DropItems handling failed");
            }
        }


        private void HandlePastedFiles(IEnumerable<IStorageItem> items)
        {
            try
            {
                _ = VidHubContext.Host.GetService<IVideoLoadService>().LoadItems(items, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to queue pasted files for loading");
            }
        }

        private void TextTrimmingChanged(TextBlock sender, IsTextTrimmedChangedEventArgs args)
        {
            try
            {
                if (sender.IsTextTrimmed)
                {
                    ToolTipService.SetToolTip(sender, sender.Text);
                }
                else
                {
                    ToolTipService.SetToolTip(sender, null);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "TextTrimmingChanged handler failed");
            }
        }

        private void OpenVideo(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Video video)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        StorageFile file = await StorageFile.GetFileFromPathAsync(video.FilePath);
                        _ = await Launcher.LaunchFileAsync(file);
                        logger.LogInformation("Opened video file from UI action: {File}", video.FilePath);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "OpenVideo action failed for {File}", video.FilePath);
                    }
                });
            }
            else
            {
                logger.LogWarning("OpenVideo invoked but DataContext is not a Video");
            }
        }
    }
}

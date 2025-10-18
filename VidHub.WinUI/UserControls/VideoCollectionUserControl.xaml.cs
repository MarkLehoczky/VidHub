using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VidHub.Core;
using VidHub.Services.Logics.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;

namespace VidHub.WinUI.UserControls
{
    public sealed partial class VideoCollectionUserControl : UserControl
    {
        public VideoCollectionUserControl()
        {
            InitializeComponent();

            var pasteAccelerator = new KeyboardAccelerator
            {
                Key = Windows.System.VirtualKey.V,
                Modifiers = Windows.System.VirtualKeyModifiers.Control
            };
            pasteAccelerator.Invoked += PasteAccelerator_Invoked;

            KeyboardAccelerators.Add(pasteAccelerator);
            KeyboardAcceleratorPlacementMode = KeyboardAcceleratorPlacementMode.Hidden;
        }


        private async void PasteAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;

            var dataPackageView = Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await dataPackageView.GetStorageItemsAsync();
                HandlePastedFiles(items);
            }
        }

        private void DragOverItems(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private async void DropItems(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync().AsTask();
                HandlePastedFiles(items);
            }
        }


        private void HandlePastedFiles(IEnumerable<IStorageItem> items)
        {
            Platform.Context.Host.GetService<IVideoLoadService>().LoadItems(items, true);
        }

        private void TextTrimmingChanged(TextBlock sender, IsTextTrimmedChangedEventArgs args)
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

        private void OpenVideo(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Video video)
            {
                Task.Run(async () =>
                {
                    var file = await StorageFile.GetFileFromPathAsync(video.FilePath);
                    await Launcher.LaunchFileAsync(file);
                });
            }
        }
    }
}

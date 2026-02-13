using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VidHub.Core;
using VidHub.Core.Models;
using VidHub.ViewModels.Dialogs;

namespace VidHub.WinUI.UserControls.Dialogs;

public sealed partial class ModifyTagsUserControl : UserControl
{
    private readonly Video video;

    public ModifyTagsUserControl(Video video)
    {
        this.video = video;
        InitializeComponent();
        if (DataContext is ModifyTagsViewModel viewModel)
        {
            viewModel.Tags.ForEach(t => t.VideoContains = video.TagID.Contains(t.ID));
        }
    }

    private void AddTagToVideo(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is Tag tag)
        {
            video.AddTag(tag);
        }
    }

    private void RemoveTagFromVideo(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is Tag tag)
        {
            video.RemoveTag(tag);
        }
    }
}

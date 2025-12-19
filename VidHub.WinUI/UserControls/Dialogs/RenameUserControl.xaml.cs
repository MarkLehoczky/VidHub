using Microsoft.UI.Xaml.Controls;
using VidHub.Core;
using VidHub.ViewModels.Dialogs;

namespace VidHub.WinUI.UserControls.Dialogs;

public sealed partial class RenameUserControl : UserControl
{
    public RenameUserControl(Video video)
    {
        InitializeComponent();
        if (DataContext is RenameViewModel viewmodel)
        {
            viewmodel.Video = video;
        }
    }
}

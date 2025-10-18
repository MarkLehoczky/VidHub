using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using VidHub.ViewModels.Modals;

namespace VidHub.WinUI.UserControls.Modals
{
    public sealed partial class VideoTitleFormatCustomizationUserControl : UserControl
    {
        public VideoTitleFormatCustomizationUserControl(Tuple<bool, IEnumerable<int>> loadContent)
        {
            InitializeComponent();
            if (DataContext is VideoTitleFormatCustomizationViewModel viewmodel)
            {
                viewmodel.TemplateMode = loadContent.Item1;
                viewmodel.ChangeVideos(loadContent.Item2);
            }
        }
    }
}

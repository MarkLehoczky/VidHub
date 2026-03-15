using Microsoft.UI.Xaml.Controls;
using VidHub.ViewModels;

namespace VidHub.WinUI.UserControls
{
    public sealed partial class SidePanelUserControl : UserControl
    {
        public SidePanelUserControl()
        {
            InitializeComponent();
        }

        private void UpdateTextFilter(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (DataContext is SidePanelViewModel viewModel)
            {
                viewModel.UpdateTextFilter();
            }
        }

        private void UpdateVideoCollection(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is SidePanelViewModel viewModel)
            {
                viewModel.Update(Core.Utilities.UpdateSection.VIDEOCOLLECTION);
            }
        }
    }
}

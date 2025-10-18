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
    }
}

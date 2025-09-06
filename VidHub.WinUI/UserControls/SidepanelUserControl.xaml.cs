using Microsoft.UI.Xaml.Controls;
using VidHub.ViewModels;

namespace VidHub.WinUI.UserControls
{
    public sealed partial class SidepanelUserControl : UserControl
    {
        public SidepanelUserControl()
        {
            InitializeComponent();
        }

        private void UpdateTextFilter(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (DataContext is SidepanelViewModel viewModel)
            {
                viewModel.UpdateTextFilter(sender.Text);
            }
        }
    }
}

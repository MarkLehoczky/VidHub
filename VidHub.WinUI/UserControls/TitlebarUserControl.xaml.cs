using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using VidHub.ViewModels;

namespace VidHub.WinUI.UserControls
{
    public sealed partial class TitleBarUserControl : UserControl
    {
        public TitleBarUserControl()
        {
            InitializeComponent();
        }


        private void UpdateDynamicSizes(object sender, PointerRoutedEventArgs e)
        {
            if (DataContext is TitleBarViewModel vm)
            {
                vm.RefreshAboutMenu();
            }
        }
    }
}

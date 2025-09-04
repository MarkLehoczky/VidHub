using Microsoft.UI.Xaml;

namespace VidHub.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
        }
    }
}

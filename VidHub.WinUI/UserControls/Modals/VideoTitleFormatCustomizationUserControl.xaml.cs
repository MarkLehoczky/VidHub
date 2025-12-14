using Microsoft.UI.Xaml.Controls;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Connectors.Modals;
using VidHub.ViewModels.Modals;
using VidHub.Core.Settings;

namespace VidHub.WinUI.UserControls.Modals
{
    public sealed partial class VideoTitleFormatCustomizationUserControl : UserControl
    {
        public VideoTitleFormatCustomizationUserControl(bool futureCustomization)
        {
            InitializeComponent();
            if (DataContext is VideoTitleFormatCustomizationViewModel viewmodel)
            {
                if (futureCustomization)
                {
                    IVideoService service = Platform.Context.Host.GetService<IVideoService>();
                    IVidHubSettings settings = Platform.Context.Host.GetService<IVidHubSettings>();
                    viewmodel.Connector = new VideoTitleLoadFormatCustomizationConnector(service, settings);
                }
                viewmodel.Connector.UpdateTitles();
            }
        }
    }
}

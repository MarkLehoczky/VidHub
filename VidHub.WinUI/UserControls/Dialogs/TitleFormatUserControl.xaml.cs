using Microsoft.UI.Xaml.Controls;
using VidHub.Core.Settings;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;
using VidHub.Services.Connectors.Dialogs;
using VidHub.ViewModels.Dialogs;

namespace VidHub.WinUI.UserControls.Dialogs
{
    public sealed partial class TitleFormatUserControl : UserControl
    {
        public TitleFormatUserControl(bool futureCustomization)
        {
            InitializeComponent();
            if (DataContext is TitleFormatViewModel viewmodel)
            {
                if (futureCustomization)
                {
                    IVideoService service = VidHubContext.Host.GetService<IVideoService>();
                    IVidHubSettings settings = VidHubContext.Host.GetService<IVidHubSettings>();
                    viewmodel.Connector = new ActiveTitleFormatConnector(service, settings);
                }
                viewmodel.Connector.UpdateTitles();
            }
        }
    }
}

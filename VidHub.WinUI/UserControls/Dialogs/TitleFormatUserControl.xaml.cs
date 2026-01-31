using Microsoft.UI.Xaml.Controls;
using VidHub.Core.Settings;
using VidHub.Services.Base;
using VidHub.ViewModels.Dialogs;
using System;
using VidHub.Services.Connectors.Dialogs;
using VidHub.Platform.VidHubEnvironment;

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

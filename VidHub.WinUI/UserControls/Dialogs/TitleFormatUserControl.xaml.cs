using Microsoft.UI.Xaml.Controls;
using VidHub.Core.Settings;
using VidHub.Services.Base;
using VidHub.ViewModels.Dialogs;
using System;
using VidHub.Services.Connectors.Dialogs;

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
                    IVideoService service = Platform.Environment.Context.Host.GetService<IVideoService>();
                    IVidHubSettings settings = Platform.Environment.Context.Host.GetService<IVidHubSettings>();
                    viewmodel.Connector = new ActiveTitleFormatConnector(service, settings);
                }
                viewmodel.Connector.UpdateTitles();
            }
        }
    }
}

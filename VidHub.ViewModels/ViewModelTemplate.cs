using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using VidHub.Core.Utilities;
using VidHub.Platform.VidHubEnvironment;
using VidHub.Services.Base;

namespace VidHub.ViewModels
{
    public abstract class ViewModelTemplate : ObservableRecipient, IDisposable
    {
        private readonly IUpdateService service;
        protected readonly ILogger logger = VidHubContext.Logger;

        public ViewModelTemplate(IUpdateService service)
        {
            this.service = service;
            this.service.SubscribeToUpdateEvent(Update);
            logger.LogTrace("ViewModelTemplate initialized and subscribed to updates");
        }

        public void Dispose()
        {
            service.UnsubscribeFromUpdateEvent(Update);
            GC.SuppressFinalize(this);
            logger.LogTrace("ViewModelTemplate disposed and unsubscribed from updates");
        }

        public abstract void Update(IEnumerable<UpdateSection> sections);
        public virtual void Update(params UpdateSection[] sections)
        {
            logger.LogTrace("Update(params) called on ViewModelTemplate with count={Count}", sections?.Length ?? 0);
            Update(sections.AsEnumerable());
        }
    }
}

using VidHub.Core.Utilities;
using VidHub.Services.Base;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Services.Connectors
{
    public class ConnectorTemplate(IUpdateService service) : IUpdateService
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public virtual void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            service.SubscribeToUpdateEvent(action);
            logger.LogTrace("SubscribeToUpdateEvent forwarded to underlying service");
        }

        public virtual void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            service.UnsubscribeFromUpdateEvent(action);
            logger.LogTrace("UnsubscribeFromUpdateEvent forwarded to underlying service");
        }

        public virtual void Update(IEnumerable<UpdateSection> sections)
        {
            service.Update(sections);
            logger.LogTrace("Update forwarded to underlying service with sections count={Count}", sections?.Count() ?? 0);
        }

        public virtual void Update(params UpdateSection[] sections)
        {
            service.Update(sections);
            logger.LogTrace("Update (params) forwarded to underlying service");
        }
    }
}

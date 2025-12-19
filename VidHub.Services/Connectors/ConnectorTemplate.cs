using VidHub.Core.Utilities;
using VidHub.Services.Base;

namespace VidHub.Services.Connectors
{
    public class ConnectorTemplate(IUpdateService service) : IUpdateService
    {
        public virtual void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            service.SubscribeToUpdateEvent(action);
        }

        public virtual void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action)
        {
            service.UnsubscribeFromUpdateEvent(action);
        }

        public virtual void Update(IEnumerable<UpdateSection> sections)
        {
            service.Update(sections);
        }

        public virtual void Update(params UpdateSection[] sections)
        {
            service.Update(sections);
        }
    }
}

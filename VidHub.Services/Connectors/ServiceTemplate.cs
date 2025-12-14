using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;

namespace VidHub.Services.Connectors
{
    public class ServiceTemplate(IUpdateService service) : IUpdateService
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

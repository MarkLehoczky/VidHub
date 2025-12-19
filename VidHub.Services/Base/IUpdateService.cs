using VidHub.Core.Utilities;

namespace VidHub.Services.Base
{
    public interface IUpdateService
    {
        void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action);
        void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action);
        void Update(IEnumerable<UpdateSection> sections);
        void Update(params UpdateSection[] sections);
    }
}

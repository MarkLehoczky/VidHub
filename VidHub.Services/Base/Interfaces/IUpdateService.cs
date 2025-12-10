using VidHub.Core.Utilities.Helper;

namespace VidHub.Services.Base.Interfaces
{
    public interface IUpdateService
    {
        void SubscribeToUpdateEvent(Action<IEnumerable<UpdateSection>> action);
        void UnsubscribeFromUpdateEvent(Action<IEnumerable<UpdateSection>> action);
        void Update(IEnumerable<UpdateSection> sections);
        void Update(params UpdateSection[] sections);
    }
}

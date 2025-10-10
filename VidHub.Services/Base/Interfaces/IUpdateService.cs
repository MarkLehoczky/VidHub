using VidHub.Core.Helpers;

namespace VidHub.Services.Base.Interfaces
{
    public interface IUpdateService
    {
        void SubscribeToUpdateEvent(Action<UpdateType> action);
        void UnsubscribeFromUpdateEvent(Action<UpdateType> action);
        void Update(UpdateType type);
    }
}

namespace VidHub.Services.Base.Interfaces
{
    public interface IUpdateService
    {
        void SubscribeToUpdateEvent(Action action);
        void UnsubscribeFromUpdateEvent(Action action);
        void Update();
    }
}

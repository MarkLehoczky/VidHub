using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Utilities.Helper;
using VidHub.Services.Base.Interfaces;

namespace VidHub.ViewModels.Base
{
    public abstract class ViewModelTemplate : ObservableRecipient, IDisposable
    {
        private readonly IUpdateService _service;

        public ViewModelTemplate(IUpdateService service)
        {
            _service = service;
            _service.SubscribeToUpdateEvent(Update);
        }

        public void Dispose()
        {
            _service.UnsubscribeFromUpdateEvent(Update);
            GC.SuppressFinalize(this);
        }

        public abstract void Update(UpdateType type);
    }
}

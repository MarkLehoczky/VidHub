using CommunityToolkit.Mvvm.ComponentModel;
using VidHub.Core.Utilities;
using VidHub.Services.Base;

namespace VidHub.ViewModels
{
    public abstract class ViewModelTemplate : ObservableRecipient, IDisposable
    {
        private readonly IUpdateService service;

        public ViewModelTemplate(IUpdateService service)
        {
            this.service = service;
            this.service.SubscribeToUpdateEvent(Update);
        }

        public void Dispose()
        {
            service.UnsubscribeFromUpdateEvent(Update);
            GC.SuppressFinalize(this);
        }

        public abstract void Update(IEnumerable<UpdateSection> sections);
        public virtual void Update(params UpdateSection[] sections)
        {
            Update(sections.AsEnumerable());
        }
    }
}

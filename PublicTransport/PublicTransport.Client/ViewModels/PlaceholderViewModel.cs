using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class PlaceholderViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Placeholder";
        public IScreen HostScreen { get; }

        public PlaceholderViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}

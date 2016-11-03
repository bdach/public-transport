using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Shell";
        public IScreen HostScreen { get; }

        public ShellViewModel(IScreen screen)
        {
            HostScreen = screen;
            HostScreen.Router.Navigate.Execute(new CityViewModel(HostScreen));
        }
    }
}
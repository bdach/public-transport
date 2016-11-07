using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Filter
{
    public class FilterTripViewModel : ReactiveObject, IDetailViewModel
    {
        public FilterTripViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Trip;
    }
}
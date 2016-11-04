using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class EditAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        public EditAgencyViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Agency;
    }
}

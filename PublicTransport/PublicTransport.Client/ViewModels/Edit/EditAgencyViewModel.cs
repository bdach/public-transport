using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    public class EditAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        public EditAgencyViewModel(IScreen screen, Agency agency = null)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Agency;
    }
}

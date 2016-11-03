using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.Interfaces
{
    public interface IDetailViewModel : IRoutableViewModel
    {
        MenuOption AssociatedMenuOption { get; }
    }
}
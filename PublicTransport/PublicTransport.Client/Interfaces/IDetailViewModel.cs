using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.Interfaces
{
    /// <summary>
    ///     Interface for detail view models. Detail view models appear on the right side of the application window and are
    ///     managed by the menu on the left.
    /// </summary>
    public interface IDetailViewModel : IRoutableViewModel
    {
        /// <summary>
        ///     Gets the <see cref="MenuOption" /> enum value that associates a menu item with the concrete view model.
        /// </summary>
        MenuOption AssociatedMenuOption { get; }
    }
}
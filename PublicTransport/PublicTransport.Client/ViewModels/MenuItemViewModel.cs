using System.Reactive.Linq;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     View model for the master view menu items.
    /// </summary>
    public class MenuItemViewModel
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="item">Menu item to be associated with this view model.</param>
        public MenuItemViewModel(MenuItem item)
        {
            Item = item;
            SelectedOption = ReactiveCommand.CreateAsyncObservable(e => Observable.Return(item));
        }

        /// <summary>
        ///     <see cref="MenuItem" /> associated with this view model.
        /// </summary>
        public MenuItem Item { get; protected set; }

        /// <summary>
        ///     Command returning the <see cref="Item" /> associated with this view model.
        /// </summary>
        public ReactiveCommand<MenuItem> SelectedOption { get; protected set; }

        /// <summary>
        ///     Returns the string representation of this item.
        /// </summary>
        /// <returns>Menu item caption.</returns>
        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
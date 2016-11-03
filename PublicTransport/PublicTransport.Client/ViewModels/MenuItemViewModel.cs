using System.Reactive.Linq;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class MenuItemViewModel
    {
        public MenuItem Item { get; protected set; }
        public ReactiveCommand<MenuItem> SelectedOption { get; protected set; }

        public MenuItemViewModel(MenuItem item)
        {
            Item = item;
            SelectedOption = ReactiveCommand.CreateAsyncObservable(e => Observable.Return(item));
        }

        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
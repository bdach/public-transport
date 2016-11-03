using System.Collections.Generic;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class MenuViewModel : ReactiveObject
    {
        private MenuItemViewModel _selectedOption;
        public ReactiveList<MenuItemViewModel> Menu { get; set; }

        public MenuItemViewModel SelectedOption
        {
            get { return _selectedOption; }
            set { this.RaiseAndSetIfChanged(ref _selectedOption, value); }
        }

        public MenuViewModel()
        {
            var cityItem = new MenuItem("Cities", MenuOption.City);
            var city = new MenuItemViewModel(cityItem);
            var streetItem = new MenuItem("Streets", MenuOption.Street);
            var street = new MenuItemViewModel(streetItem);
            Menu = new ReactiveList<MenuItemViewModel> {city, street};
        }
    }
}
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    /// View model for the master view (menu).
    /// </summary>
    public class MenuViewModel : ReactiveObject
    {
        /// <summary>
        /// Currently selected menu item, represented as a <see cref="MenuItemViewModel"/>.
        /// </summary>
        private MenuItemViewModel _selectedOption;
        /// <summary>
        /// List of all menu items.
        /// </summary>
        public ReactiveList<MenuItemViewModel> Menu { get; set; }

        /// <summary>
        /// Returns the currently selected menu item.
        /// </summary>
        public MenuItemViewModel SelectedOption
        {
            get { return _selectedOption; }
            set { this.RaiseAndSetIfChanged(ref _selectedOption, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuViewModel()
        {
            // TODO: Preferably change this to something more resilient and flexible.
            var cityItem = new MenuItem("Cities", MenuOption.City);
            var city = new MenuItemViewModel(cityItem);
            var streetItem = new MenuItem("Streets", MenuOption.Street);
            var street = new MenuItemViewModel(streetItem);
            var agencyItem = new MenuItem("Agencies", MenuOption.Agency);
            var agency = new MenuItemViewModel(agencyItem);
            var zoneItem = new MenuItem("Zones", MenuOption.Zone);
            var zone = new MenuItemViewModel(zoneItem);
            Menu = new ReactiveList<MenuItemViewModel> {city, street, agency, zone};
        }
    }
}
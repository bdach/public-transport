using System;
using System.Collections.Generic;
using System.Linq;
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
            var menuItems = new List<Tuple<string, MenuOption>>
            {
                new Tuple<string, MenuOption>("Cities", MenuOption.City),
                new Tuple<string, MenuOption>("Stops", MenuOption.Street),
                new Tuple<string, MenuOption>("Agencies", MenuOption.Agency),
                new Tuple<string, MenuOption>("Routes", MenuOption.Route),
                new Tuple<string, MenuOption>("Stops", MenuOption.Stop),
                new Tuple<string, MenuOption>("Zones", MenuOption.Zone),
                new Tuple<string, MenuOption>("Fares", MenuOption.Fare)
            };
            Menu =
                new ReactiveList<MenuItemViewModel>(
                    menuItems.Select(item => new MenuItemViewModel(new MenuItem(item.Item1, item.Item2))));
        }
    }
}
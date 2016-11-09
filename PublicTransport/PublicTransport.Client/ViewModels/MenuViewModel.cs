using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     View model for the master view (menu).
    /// </summary>
    public class MenuViewModel : ReactiveObject
    {
        /// <summary>
        ///     Currently selected menu item, represented as a <see cref="MenuItemViewModel" />.
        /// </summary>
        private MenuItemViewModel _selectedOption;

        /// <summary>
        ///     <see cref="Services.DataTransfer.UserInfo" /> object containing data about the currently logged in user.
        /// </summary>
        private UserInfo _userInfo;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public MenuViewModel()
        {
            Menu = new ReactiveList<MenuItemViewModel>();

            #region Populating menu upon login

            this.WhenAnyValue(vm => vm.UserInfo)
                .Where(ui => ui != null)
                .Subscribe(_ =>
                {
                    Menu.Clear();
                    Menu.AddRange(GetMenuItems(UserInfo));
                });

            #endregion

            #region Logging out

            LogOut = ReactiveCommand.Create();
            LogOut.Subscribe(_ =>
            {
                Menu.Clear();
                UserInfo = null;
            });

            #endregion
        }

        /// <summary>
        ///     List of all menu items.
        /// </summary>
        public ReactiveList<MenuItemViewModel> Menu { get; set; }

        /// <summary>
        ///     Returns the currently selected menu item.
        /// </summary>
        public MenuItemViewModel SelectedOption
        {
            get { return _selectedOption; }
            set { this.RaiseAndSetIfChanged(ref _selectedOption, value); }
        }

        /// <summary>
        ///     <see cref="Services.DataTransfer.UserInfo" /> object containing data about the currently logged in user.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set { this.RaiseAndSetIfChanged(ref _userInfo, value); }
        }

        /// <summary>
        ///     Logs the current user out of the application.
        /// </summary>
        public ReactiveCommand<object> LogOut { get; protected set; }

        /// <summary>
        ///     Returns a list of menu items applicable for the given user. The items' availability is determined by user roles.
        /// </summary>
        /// <param name="userInfo">Object containing user info data.</param>
        /// <returns>An enumerable of menu items to insert to the menu item list.</returns>
        public IEnumerable<MenuItemViewModel> GetMenuItems(UserInfo userInfo)
        {
            // TODO: Preferably change this to something more resilient and flexible.
            var userMenuItems = new List<Tuple<RoleType, string, MenuOption>>
            {
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Cities", MenuOption.City),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Streets", MenuOption.Street),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Agencies", MenuOption.Agency),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Routes", MenuOption.Route),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Stops", MenuOption.Stop),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Zones", MenuOption.Zone),
                new Tuple<RoleType, string, MenuOption>(RoleType.Employee, "Fares", MenuOption.Fare),
                new Tuple<RoleType, string, MenuOption>(RoleType.Administrator, "Users", MenuOption.User)
            };
            return userMenuItems.Where(s => userInfo.UserRoles.Any(ui => ui == s.Item1))
                .Select(s => new MenuItem(s.Item2, s.Item3))
                .Select(item => new MenuItemViewModel(item));
        }
    }
}
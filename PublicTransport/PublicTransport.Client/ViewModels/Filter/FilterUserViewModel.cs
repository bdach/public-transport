using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Users;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering users.
    /// </summary>
    public class FilterUserViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        ///     <see cref="DataTransfer.UserReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        private UserReactiveFilter _userReactiveFilter;

        /// <summary>
        ///     The <see cref="UserDto" /> currently selected by the user.
        /// </summary>
        private UserDto _selectedUser;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="userService">Service used in the view model to access the database.</param>
        public FilterUserViewModel(IScreen screen, IUserService userService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            _userReactiveFilter = new UserReactiveFilter();
            Users = new ReactiveList<UserDto>();
            Roles = new ReactiveList<RoleType>(Enum.GetValues(typeof(RoleType)).Cast<RoleType>());

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedUser).Select(s => s != null);

            #region User filtering command

            FilterUsers = ReactiveCommand.CreateAsyncTask(async _ => await _userService.FilterUsersAsync(UserReactiveFilter.Convert()));
            FilterUsers.Subscribe(result =>
            {
                Users.Clear();
                Users.AddRange(result);
            });
            FilterUsers.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch user data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered users upon filter change

            this.WhenAnyValue(
                    vm => vm.UserReactiveFilter.UserNameFilter,
                    vm => vm.UserReactiveFilter.RoleTypeFilter)
                .Where(_ => UserReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterUsers);

            #endregion

            #region Delete user command

            DeleteUser = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await _userService.DeleteUserAsync(SelectedUser);
                return Unit.Default;
            });
            DeleteUser.Subscribe(_ => SelectedUser = null);
            DeleteUser.InvokeCommand(FilterUsers);
            DeleteUser.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected user. Please contact the system administrator.", e));

            #endregion

            #region Add/edit user commands

            AddUser = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditUserViewModel(HostScreen, _userService)));
            EditUser = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditUserViewModel(HostScreen, _userService, SelectedUser)));

            #endregion

            #region Clearing enum choice

            ClearRoleTypeChoice = ReactiveCommand.Create();
            ClearRoleTypeChoice.Subscribe(_ => UserReactiveFilter.RoleTypeFilter = null);

            #endregion

            #region Updating the list of users upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && UserReactiveFilter.IsValid)
                .InvokeCommand(FilterUsers);

            #endregion
        }

        /// <summary>
        ///     The <see cref="UserDto" /> currently selected by the user.
        /// </summary>
        public UserDto SelectedUser
        {
            get { return _selectedUser; }
            set { this.RaiseAndSetIfChanged(ref _selectedUser, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.UserReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        public UserReactiveFilter UserReactiveFilter
        {
            get { return _userReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _userReactiveFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="RoleType"/>s to filter users by.
        /// </summary>
        public ReactiveList<RoleType> Roles { get; protected set; }

        /// <summary>
        ///     The list of <see cref="UserDto" /> objects currently displayed by the user.
        /// </summary>
        public ReactiveList<UserDto> Users { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="UserDto" /> objects from the database, using the <see cref="DataTransfer.UserReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<UserDto[]> FilterUsers { get; protected set; }

        /// <summary>
        ///     Clears the role type filter.
        /// </summary>
        public ReactiveCommand<object> ClearRoleTypeChoice { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for adding a new <see cref="User" /> to the database.
        /// </summary>
        public ReactiveCommand<object> AddUser { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for editing a <see cref="User" />.
        /// </summary>
        public ReactiveCommand<object> EditUser { get; protected set; }

        /// <summary>
        ///     Deletes the currently selected <see cref="User" />.
        /// </summary>
        public ReactiveCommand<Unit> DeleteUser { get; protected set; }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => AssociatedMenuOption.ToString();

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     Gets the <see cref="MenuOption" /> enum value that associates a menu item with the concrete view model.
        /// </summary>
        public MenuOption AssociatedMenuOption => MenuOption.User;
    }
}

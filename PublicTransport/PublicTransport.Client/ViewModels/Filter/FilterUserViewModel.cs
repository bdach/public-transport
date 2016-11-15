﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.UnitsOfWork;
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
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly IUserUnitOfWork _userUnitOfWork;

        /// <summary>
        ///     <see cref="DataTransfer.UserFilter" /> object used to send query data to the service layer.
        /// </summary>
        private UserFilter _userFilter;

        /// <summary>
        ///     The <see cref="User" /> currently selected by the user.
        /// </summary>
        private User _selectedUser;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="userUnitOfWork">Unit of work used in the view model to access the database.</param>
        public FilterUserViewModel(IScreen screen, IUserUnitOfWork userUnitOfWork = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _userUnitOfWork = userUnitOfWork ?? Locator.Current.GetService<IUserUnitOfWork>();
            _userFilter = new UserFilter();
            Users = new ReactiveList<User>();
            Roles = new ReactiveList<RoleType>(Enum.GetValues(typeof(RoleType)).Cast<RoleType>());

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedUser).Select(s => s != null);

            #region User filtering command

            FilterUsers = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _userUnitOfWork.FilterUsers(UserFilter)));
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
                    vm => vm.UserFilter.UserNameFilter,
                    vm => vm.UserFilter.RoleTypeFilter)
                .Where(_ => UserFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterUsers);

            #endregion

            #region Delete user command

            DeleteUser = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _userUnitOfWork.DeleteUser(SelectedUser));
                return Unit.Default;
            });
            DeleteUser.Subscribe(_ => SelectedUser = null);
            DeleteUser.InvokeCommand(FilterUsers);
            DeleteUser.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected user. Please contact the system administrator.", e));

            #endregion

            #region Add/edit user commands

            AddUser = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditUserViewModel(HostScreen, _userUnitOfWork)));
            EditUser = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditUserViewModel(HostScreen, _userUnitOfWork, SelectedUser)));

            #endregion

            #region Clearing enum choice

            ClearRoleTypeChoice = ReactiveCommand.Create();
            ClearRoleTypeChoice.Subscribe(_ => UserFilter.RoleTypeFilter = null);

            #endregion

            #region Updating the list of users upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && UserFilter.IsValid)
                .InvokeCommand(FilterUsers);

            #endregion

            #region Disposing of contexts

            HostScreen.Router.NavigateAndReset
                .Skip(1)
                .Subscribe(_ => _userUnitOfWork.Dispose());

            #endregion
        }

        /// <summary>
        ///     The <see cref="User" /> currently selected by the user.
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { this.RaiseAndSetIfChanged(ref _selectedUser, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.UserFilter" /> object used to send query data to the service layer.
        /// </summary>
        public UserFilter UserFilter
        {
            get { return _userFilter; }
            set { this.RaiseAndSetIfChanged(ref _userFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="RoleType"/>s to filter users by.
        /// </summary>
        public ReactiveList<RoleType> Roles { get; protected set; }

        /// <summary>
        ///     The list of <see cref="Users" /> objects currently displayed by the user.
        /// </summary>
        public ReactiveList<User> Users { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="User" /> objects from the database, using the <see cref="DataTransfer.UserFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<List<User>> FilterUsers { get; protected set; }

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

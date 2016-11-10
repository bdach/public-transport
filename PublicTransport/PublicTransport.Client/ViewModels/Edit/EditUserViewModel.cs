using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Entities;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.User"/> objects.
    /// </summary>
    public class EditUserViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly UserUnitOfWork _userUnitOfWork;

        /// <summary>
        ///     The <see cref="Domain.Entities.User" /> object being edited in the window.
        /// </summary>
        private User _user;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="userUnitOfWork">Unit of work exposing methods necessary to manage data.</param>
        /// <param name="user">User to be edited. If a user is to be added, this parameter should be left null.</param>
        public EditUserViewModel(IScreen screen, UserUnitOfWork userUnitOfWork, User user = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _userUnitOfWork = userUnitOfWork;

            var serviceMethod = user == null ? new Func<User, User>(_userUnitOfWork.CreateUser) : _userUnitOfWork.UpdateUser;
            _user = user ?? new User();
            RoleViewModels = new ReactiveList<RoleViewModel>();

            #endregion

            #region GetRoles command

            GetRoles = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _userUnitOfWork.GetAllRoles()));
            GetRoles.Subscribe(result => RoleViewModels.AddRange(result.Select(r => new RoleViewModel(r, User.Roles.Any(ur => ur.Name == r.Name))).ToList()));

            #endregion
            
            #region SaveUser command

            SaveUser = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                User.Roles = RoleViewModels.Where(vm => vm.Selected).Select(vm => vm.Role).ToList();
                return await Task.Run(() => serviceMethod(User));
            });
            SaveUser.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited user cannot be saved to the database. Please contact the system administrator.", ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Command responsible for populating user roles.
        /// </summary>
        public ReactiveCommand<List<Role>> GetRoles { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.User"/> objects.
        /// </summary>
        public ReactiveCommand<User> SaveUser { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The list of <see cref="Role" /> values.
        /// </summary>
        public ReactiveList<RoleViewModel> RoleViewModels { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.User" /> object being edited in the window.
        /// </summary>
        public User User
        {
            get { return _user; }
            set { this.RaiseAndSetIfChanged(ref _user, value); }
        }

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

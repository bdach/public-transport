using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Users;
using PublicTransport.Client.ViewModels.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.User"/> objects.
    /// </summary>
    public class EditUserViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        ///     The <see cref="UserDto" /> object being edited in the window.
        /// </summary>
        private UserDto _user;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="userService">Service exposing methods necessary to manage data.</param>
        /// <param name="user">User to be edited. If a user is to be added, this parameter should be left null.</param>
        public EditUserViewModel(IScreen screen, IUserService userService, UserDto user = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _userService = userService;

            var serviceMethod = user == null ? new Func<UserDto, Task<UserDto>>(_userService.CreateUserAsync) : _userService.UpdateUserAsync;
            _user = user ?? new UserDto();
            RoleViewModels = new ReactiveList<RoleViewModel>();

            #endregion

            #region GetRoles command

            GetRoles = ReactiveCommand.CreateAsyncTask(async _ => await _userService.GetAllRolesAsync());
            GetRoles.Subscribe(result => RoleViewModels.AddRange(result.Select(r => new RoleViewModel(r, User.Roles.Any(ur => ur.Name == r.Name))).ToList()));

            #endregion
            
            #region SaveUser command

            SaveUser = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                User.Roles = RoleViewModels.Where(vm => vm.Selected).Select(vm => vm.Role).ToList();
                return await serviceMethod(User);
            });
            SaveUser.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveUser.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Command responsible for populating user roles.
        /// </summary>
        public ReactiveCommand<RoleDto[]> GetRoles { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.User"/> objects.
        /// </summary>
        public ReactiveCommand<UserDto> SaveUser { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The list of <see cref="RoleDto" /> values.
        /// </summary>
        public ReactiveList<RoleViewModel> RoleViewModels { get; protected set; }

        /// <summary>
        ///     The <see cref="UserDto" /> object being edited in the window.
        /// </summary>
        public UserDto User
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

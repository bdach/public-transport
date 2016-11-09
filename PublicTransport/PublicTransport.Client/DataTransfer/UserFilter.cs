using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="UserService" /> to perform filtering.
    /// </summary>
    public class UserFilter : ReactiveObject, IUserFilter, IReactiveFilter
    {
        /// <summary>
        ///     Contains the username string filter parameter.
        /// </summary>
        private string _userNameFilter = "";

        /// <summary>
        ///     Contains the role name enum filter parameter.
        /// </summary>
        private RoleType? _roleNameFilter;

        /// <summary>
        ///     Contains the username string filter parameter.
        /// </summary>
        public string UserNameFilter
        {
            get { return _userNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _userNameFilter, value); }
        }

        /// <summary>
        ///     Contains the role name enum filter parameter.
        /// </summary>
        public RoleType? RoleNameFilter
        {
            get { return _roleNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _roleNameFilter, value); }
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(_userNameFilter) || RoleNameFilter.HasValue;
    }
}

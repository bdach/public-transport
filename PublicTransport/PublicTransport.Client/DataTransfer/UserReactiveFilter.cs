using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.User" /> objects.
    /// </summary>
    public class UserReactiveFilter : ReactiveObject, IReactiveFilter
    {
        /// <summary>
        ///     Contains the username string filter parameter.
        /// </summary>
        private string _userNameFilter = "";

        /// <summary>
        ///     Contains the role name enum filter parameter.
        /// </summary>
        private RoleType? _roleTypeFilter;

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
        public RoleType? RoleTypeFilter
        {
            get { return _roleTypeFilter; }
            set { this.RaiseAndSetIfChanged(ref _roleTypeFilter, value); }
        }

        public UserFilter Convert()
        {
            return new UserFilter
            {
                UserNameFilter = UserNameFilter,
                RoleTypeFilter = RoleTypeFilter
            };
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(_userNameFilter) ||
            RoleTypeFilter.HasValue;
    }
}

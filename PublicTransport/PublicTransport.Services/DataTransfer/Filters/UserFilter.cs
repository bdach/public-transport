using PublicTransport.Domain.Enums;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="UserRepository" /> to perform filtering.
    /// </summary>
    public class UserFilter
    {
        /// <summary>
        ///     Contains the username string filter parameter.
        /// </summary>
        public string UserNameFilter { get; set; }

        /// <summary>
        ///     Contains the role name enum filter parameter.
        /// </summary>
        public RoleType? RoleTypeFilter { get; set; }
    }
}

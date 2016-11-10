using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="UserService" /> to perform filtering.
    /// </summary>
    public interface IUserFilter
    {
        /// <summary>
        ///     Contains the username string filter parameter.
        /// </summary>
        string UserNameFilter { get; }

        /// <summary>
        ///     Contains the role name enum filter parameter.
        /// </summary>
        RoleType? RoleNameFilter { get; }
    }
}

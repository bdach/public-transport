using PublicTransport.Domain.Entities;

namespace PublicTransport.Domain.Enums
{
    /// <summary>
    ///     Describes the <see cref="User"/> roles in the system.
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        ///     Gives <see cref="User"/> access to parts of the system available only for the employees.
        /// </summary>
        Employee,

        /// <summary>
        ///     Gives <see cref="User"/> access to parts of the system available only for the administator.
        /// </summary>
        Administator
    }
}

using System.Collections.Generic;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Describes the <see cref="User"/> roles in the system.
    /// </summary>
    public class Role : Entity
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public Role()
        {
            Users = new List<User>();
        }

        /// <summary>
        ///     Defines the role type.
        /// </summary>
        public RoleType Name { get; set; }

        /// <summary>
        ///     Return a list of <see cref="User"/>s which have this role assigned.
        /// </summary>
        public IList<User> Users { get; set; }
    }
}

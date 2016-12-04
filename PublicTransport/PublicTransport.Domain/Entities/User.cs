using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a user of the system.
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public User()
        {
            Roles = new List<Role>();
        }

        /// <summary>
        ///     Contains the username (login) of the user.
        /// </summary>
        [Required(ErrorMessage = "The username is required.")]
        public string UserName { get; set; }

        /// <summary>
        ///     Contains the password of the user.
        /// </summary>
        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="Role"/>s assigned to the user.
        /// </summary>
        public IList<Role> Roles { get; set; }
    }
}

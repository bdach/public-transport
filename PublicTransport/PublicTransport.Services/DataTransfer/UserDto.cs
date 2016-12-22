using System.Collections.Generic;
using System.Runtime.Serialization;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.User"/> objects.
    /// </summary>
    [DataContract]
    public class UserDto
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public UserDto()
        {
            Roles = new List<RoleDto>();
        }

        /// <summary>
        /// User ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Contains the full name of the user.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        ///     Contains the username (login) of the user.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        ///     Contains the password of the user.
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="Role"/>s assigned to the user.
        /// </summary>
        [DataMember]
        public IList<RoleDto> Roles { get; set; }
    }
}
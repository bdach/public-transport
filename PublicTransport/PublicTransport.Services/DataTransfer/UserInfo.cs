using System.Collections.Generic;
using System.Runtime.Serialization;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object exposing the minimum amount of data necessary to enforce rules in the client application.
    /// </summary>
    [DataContract]
    public class UserInfo
    {
        public UserInfo() { }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="userName">User name of the user that logged into the application.</param>
        /// <param name="userRoles">Roles of the user that logged into the application.</param>
        public UserInfo(string userName, IList<RoleType> userRoles)
        {
            UserName = userName;
            UserRoles = userRoles;
        }

        /// <summary>
        ///     User name of the user that logged into the application.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        ///     Roles of the user that logged into the application.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IList<RoleType> UserRoles { get; set; }
    }
}
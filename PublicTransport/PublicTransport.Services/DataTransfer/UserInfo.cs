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
        /// <param name="roles">Roles of the user that logged into the application.</param>
        public UserInfo(string userName, IList<RoleType> roles)
        {
            UserName = userName;
            Roles = roles;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="fullName">Full name of the user that logged into the web application.</param>
        /// <param name="userName">User name of the user that logged into the application.</param>
        /// <param name="token">Latest token of the user that logged into the web application granted by OAuth.</param>
        /// <param name="roles">Roles of the user that logged into the application.</param>
        public UserInfo(string fullName, string userName, string token, IList<RoleType> roles)
        {
            FullName = fullName;
            UserName = userName;
            Token = token;
            Roles = roles;
        }

        /// <summary>
        ///     Full name of the user that logged into the web application.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        ///     User name of the user that logged into the application.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        ///     Roles of the user that logged into the application.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IList<RoleType> Roles { get; set; }

        /// <summary>
        ///     Token of the user that logged into the web application granted by OAuth.
        /// </summary>
        [DataMember]
        public string Token { get; set; }
    }
}
﻿using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object for client applications. Used to transfer login credentials to the database.
    /// </summary>
    [DataContract]
    public class LoginData
    {
        public LoginData() { }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="userName">Username of the user logging in.</param>
        /// <param name="password">Password supplied by the user.</param>
        public LoginData(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        ///     Username of the user logging in.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        ///     Password supplied by the user.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set;  }
    }
}
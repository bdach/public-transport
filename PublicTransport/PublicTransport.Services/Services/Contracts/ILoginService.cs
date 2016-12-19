using System;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing login data.
    /// </summary>
    [ServiceContract]
    public interface ILoginService : IDisposable
    {
        /// <summary>
        ///     Handles a user login request.
        /// </summary>
        /// <param name="loginData">Object containing the credentials: username and hashed password.</param>
        /// <returns>A <see cref="LoginData" /> object containing minimum required information about the user who logged in.</returns>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials supplied by the user were invalid.</exception>
        [OperationContract]
        UserInfo RequestLogin(LoginData loginData);
    }

}

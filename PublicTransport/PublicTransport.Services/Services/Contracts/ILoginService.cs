using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing login data.
    /// </summary>
    [ServiceContract]
    public interface ILoginService
    {
        /// <summary>
        ///     Handles a user login request.
        /// </summary>
        /// <param name="loginData">Object containing the credentials: username and hashed password.</param>
        /// <returns>A <see cref="LoginData" /> object containing minimum required information about the user who logged in.</returns>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials supplied by the user were invalid.</exception>
        [OperationContract]
        UserInfo RequestLogin(LoginData loginData);

        /// <summary>
        ///     Handles a password change request.
        /// </summary>
        /// <param name="data">Object containing the credentials: username, old password, new password.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials supplied by the user were invalid.</exception>
        void RequestPasswordChange(PasswordChangeData data);

        /// <summary>
        ///     Validates user credentials.
        /// </summary>
        /// <param name="loginData">Object containing the credentials: username and hashed password.</param>
        /// <returns>A boolean value representing the validity of credentials.</returns>
        bool ValidateCredentials(LoginData loginData);

        /// <summary>
        ///     Updates latest token granted to user by OAuth.
        /// </summary>
        /// <param name="userName">Username (login) of the user to have token updated.</param>
        /// <param name="token">New token granted to user.</param>
        void UpdateUserToken(string userName, string token);

        /// <summary>
        ///     Retrieves user info.
        /// </summary>
        /// <param name="userName">Username of the user to find.</param>
        /// <returns><see cref="UserInfo"/> object containing user information.</returns>
        UserInfo GetUserInfoByUserName(string userName);

        /// <summary>
        ///     Retrieves user info.
        /// </summary>
        /// <param name="token">Token of the user to find.</param>
        /// <returns><see cref="UserInfo"/> object containing user information.</returns>
        /// <exception cref="EntryNotFoundException">Thrown when the user could not be found.</exception>
        UserInfo GetUserInfoByToken(string token);
    }

}

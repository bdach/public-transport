using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used for handling user logins.
    /// </summary>
    public class LoginService : ILoginService
    {

        /// <summary>
        ///     Service providing password hashing capabilities.
        /// </summary>
        private readonly IPasswordService _passwordService;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public LoginService()
        {
            _passwordService = new PasswordService();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="passwordService">Password service to use for hashing.</param>
        public LoginService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        /// <summary>
        ///     Handles a user login request.
        /// </summary>
        /// <param name="loginData">Object containing the credentials: username and hashed password.</param>
        /// <returns>A <see cref="LoginData" /> object containing minimum required information about the user who logged in.</returns>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials supplied by the user were invalid.</exception>
        public UserInfo RequestLogin(LoginData loginData)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserName == loginData.UserName);
                if (user == null || !_passwordService.CompareWithHash(loginData.Password, user.Password))
                {
                    throw new InvalidCredentialsException();
                }

                return new UserInfo(user.UserName, user.Roles.Select(r => r.Name).ToList());
            }
        }

        /// <summary>
        ///     Handles a password change request.
        /// </summary>
        /// <param name="data">Object containing the credentials: username, old password, new password.</param>
        /// <exception cref="InvalidCredentialsException">Thrown when the credentials supplied by the user were invalid.</exception>
        public void RequestPasswordChange(PasswordChangeData data)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == data.UserName);
                if (user == null || !_passwordService.CompareWithHash(data.OldPassword, user.Password))
                {
                    throw new InvalidCredentialsException();
                }

                var userRepository = new UserRepository(db);
                user.Password = data.NewPassword;
                userRepository.Update(user);
            }
        }

        /// <summary>
        ///     Validates user credentials.
        /// </summary>
        /// <param name="loginData">Object containing the credentials: username and hashed password.</param>
        /// <returns>A boolean value representing the validity of credentials.</returns>
        public bool ValidateCredentials(LoginData loginData)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == loginData.UserName);
                return user != null && _passwordService.CompareWithHash(loginData.Password, user.Password);
            }
        }

        /// <summary>
        ///     Updates latest token granted to user by OAuth.
        /// </summary>
        /// <param name="userName">Username (login) of the user to have token updated.</param>
        /// <param name="token">New token granted to user.</param>
        public void UpdateUserToken(string userName, string token)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.First(u => u.UserName == userName);
                var userRepository = new UserRepository(db);
                user.LatestToken = token;
                userRepository.SimpleUpdate(user);
            }
        }

        /// <summary>
        ///     Retrieves user info.
        /// </summary>
        /// <param name="userName">Username of the user to find.</param>
        /// <returns><see cref="UserInfo"/> object containing user information.</returns>
        public UserInfo GetUserInfoByUserName(string userName)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.Include(u => u.Roles).First(u => u.UserName == userName);
                return new UserInfo(user.FullName, user.UserName, user.LatestToken, user.Roles.Select(r => r.Name).ToList());
            }
        }
    }
}
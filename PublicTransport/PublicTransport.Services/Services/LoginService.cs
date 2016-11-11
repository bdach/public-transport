using System;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used for handling user logins.
    /// </summary>
    public class LoginService : IDisposable
    {
        /// <summary>
        ///     Context to use when logging in.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Service providing password hashing capabilities.
        /// </summary>
        private readonly IPasswordService _passwordService;

        /// <summary>
        ///     Indicates whether the <see cref="Dispose" /> method has been called.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public LoginService()
        {
            _db = new PublicTransportContext();
            _passwordService = new PasswordService();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db">Database context to be injected to the service.</param>
        /// <param name="passwordService">Password service to use for hashing.</param>
        public LoginService(PublicTransportContext db, IPasswordService passwordService)
        {
            _db = db;
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
            var user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserName == loginData.UserName);
            if (user == null || !_passwordService.CompareWithHash(loginData.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }
            return new UserInfo(user.UserName, user.Roles.Select(r => r.Name).ToList());
        }

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}
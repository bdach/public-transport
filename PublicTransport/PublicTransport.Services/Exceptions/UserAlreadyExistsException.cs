using System;

namespace PublicTransport.Services.Exceptions
{
    /// <summary>
    /// Thrown when a user with provided username already exists in the database.
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public UserAlreadyExistsException() : base("User with that username already exists.") { }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">Exception message to display.</param>
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}

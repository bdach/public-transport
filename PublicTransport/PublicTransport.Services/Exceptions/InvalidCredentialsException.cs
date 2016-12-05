using System.ServiceModel;

namespace PublicTransport.Services.Exceptions
{
    /// <summary>
    ///     Exception indicating that the login credentials supplied by the user were incorrect.
    /// </summary>
    public class InvalidCredentialsException : FaultException
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public InvalidCredentialsException() : base("The supplied login and password combination is not valid.") { }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">Exception message to display.</param>
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
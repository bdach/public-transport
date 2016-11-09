namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object for client applications. Used to transfer login credentials to the database.
    /// </summary>
    public class LoginData
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="userName">Username of the user logging in.</param>
        /// <param name="passwordHash">Hash of the user's password.</param>
        public LoginData(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }

        /// <summary>
        ///     Username of the user logging in.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        ///     Hash of the user's password.
        /// </summary>
        public string PasswordHash { get; }
    }
}
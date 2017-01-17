namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object used by the web application to change user's password.
    /// </summary>
    public class PasswordChangeData
    {
        /// <summary>
        ///     Contains the username (login) of the user logged in to the web application.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Contains the old password of the user logged in to the web application.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        ///     Contains the new password of the user logged in to the web application.
        /// </summary>
        public string NewPassword { get; set; }
    }
}

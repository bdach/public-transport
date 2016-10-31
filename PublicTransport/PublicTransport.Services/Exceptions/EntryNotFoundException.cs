using System;

namespace PublicTransport.Services.Exceptions
{
    /// <summary>
    /// Thrown when an entry cannot be located in the database.
    /// </summary>
    public class EntryNotFoundException : ApplicationException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryNotFoundException() : base("The supplied entry could not be found in the database.") {}
    }
}

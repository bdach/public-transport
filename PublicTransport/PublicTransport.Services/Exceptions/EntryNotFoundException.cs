using System.ServiceModel;

namespace PublicTransport.Services.Exceptions
{
    /// <summary>
    /// Thrown when an entry cannot be located in the database.
    /// </summary>
    public class EntryNotFoundException : FaultException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntryNotFoundException() : base("The supplied entry could not be found in the database.") {}
    }
}

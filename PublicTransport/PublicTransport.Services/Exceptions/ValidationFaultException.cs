using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace PublicTransport.Services.Exceptions
{
    public class ValidationFaultException : FaultException<ValidationFault>
    {
        public ValidationFaultException(DbEntityValidationException exception) :
            base(new ValidationFault(exception),
                "There were one or more validation errors while saving the changes to database.")
        { }
    }

    [DataContract]
    public class ValidationFault
    {
        public ValidationFault()  { }

        public ValidationFault(DbEntityValidationException exception)
        {
            Errors = exception
                .EntityValidationErrors
                .SelectMany(vr => vr.ValidationErrors.Select(ve => ve.ErrorMessage))
                .ToArray();
        }

        [DataMember]
        public string[] Errors { get; set; }
    }
}
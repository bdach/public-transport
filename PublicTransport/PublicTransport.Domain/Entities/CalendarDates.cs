using System;
using System.ComponentModel.DataAnnotations;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Defines exceptional dates when a <code>Trip</code> is operated, as well as when it is not operated.
    /// </summary>
    public class CalendarDates : Entity
    {
        /// <summary>
        ///     Specifies a particular date when service availability is different than the norm.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Indicates whether service is available on the date specified in the <see cref="Date" /> field.
        /// </summary>
        [Required]
        public ExceptionType ExceptionType { get; set; }
    }
}
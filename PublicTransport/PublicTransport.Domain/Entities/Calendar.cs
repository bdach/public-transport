using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Defines a range of days between which a <see cref="Trip" /> is available and the days of the week when it is
    ///     available.
    /// </summary>
    public class Calendar : Entity
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public Calendar()
        {
            CalendarDates = new List<CalendarDate>();
        }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Mondays in the date range.
        /// </summary>
        [Required]
        public bool Monday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Tuesdays in the date range.
        /// </summary>
        [Required]
        public bool Tuesday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Wednesdays in the date range.
        /// </summary>
        [Required]
        public bool Wednesday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Thursdays in the date range.
        /// </summary>
        [Required]
        public bool Thursday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Fridays in the date range.
        /// </summary>
        [Required]
        public bool Friday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Saturdays in the date range.
        /// </summary>
        [Required]
        public bool Saturday { get; set; }
        
        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Sundays in the date range.
        /// </summary>
        [Required]
        public bool Sonday { get; set; }

        /// <summary>
        ///     Contains the start date for the service.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Contains the end date for the service.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="CalendarDate" />s that this calendar references.
        /// </summary>
        public IList<CalendarDate> CalendarDates { get; set; }
    }
}
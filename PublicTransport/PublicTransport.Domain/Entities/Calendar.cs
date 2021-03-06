﻿using System;
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
        ///     Contains a binary value that indicates whether the service is valid for all Mondays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Mondays.")]
        public bool Monday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Tuesdays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Tuesdays.")]
        public bool Tuesday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Wednesdays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Wednesdays.")]
        public bool Wednesday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Thursdays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Thursdays.")]
        public bool Thursday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Fridays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Fridays.")]
        public bool Friday { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Saturdays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Saturdays.")]
        public bool Saturday { get; set; }
        
        /// <summary>
        ///     Contains a binary value that indicates whether the service is valid for all Sundays in the date range.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the trip runs on Sundays.")]
        public bool Sunday { get; set; }

        /// <summary>
        ///     Contains the start date for the service.
        /// </summary>
        [Required(ErrorMessage = "The start date is required.")]
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Contains the end date for the service.
        /// </summary>
        [Required(ErrorMessage = "The end date is required.")]
        public DateTime EndDate { get; set; }
    }
}
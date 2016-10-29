using System;
using System.ComponentModel.DataAnnotations;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    public class CalendarDates : Entity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ExceptionType ExceptionType { get; set; }
    }
}

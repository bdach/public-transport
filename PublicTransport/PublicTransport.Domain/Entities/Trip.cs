using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a journey taken by a vehicle through Stops. Trips are time-specific - they are defined as a sequence of
    ///     StopTimes, so a single Trip represents one journey along a transit line or route. In addition to StopTimes, Trips
    ///     use <see cref="Calendar" />s to define the days when a Trip is available to passengers.
    /// </summary>
    public class Trip : Entity
    {
        /// <summary>
        ///     Contains an ID that uniquely identifies a route.
        /// </summary>
        [Required(ErrorMessage = "The trip must be associated with a city.")]
        public int RouteId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Entities.Route" /> along which the Trip takes place.
        /// </summary>
        [ForeignKey("RouteId")]
        public Route Route { get; set; }

        /// <summary>
        ///     Contains an ID that uniquely identifies a set of dates when service is available for one or more routes.
        /// </summary>
        [Required(ErrorMessage = "The trip must have a defined service pattern.")]
        public int ServiceId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Calendar" /> object that contains the information about the trip's availability.
        /// </summary>
        [ForeignKey("ServiceId")]
        public Calendar Service { get; set; }

        /// <summary>
        ///     Contains the text that appears on a sign that identifies the trip's destination to passengers. Use this field to
        ///     distinguish between different patterns of service in the same route.
        /// </summary>
        public string Headsign { get; set; }

        /// <summary>
        ///     Contains the text that appears in schedules and sign boards to identify the trip to passengers, for example, to
        ///     identify train numbers for commuter rail trips. If riders do not commonly rely on trip names, please leave this
        ///     field blank.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///     Contains a binary value that indicates the direction of travel for a trip. Use this field to distinguish between
        ///     bi-directional trips with the same <see cref="RouteId" />. This field is not used for routing; it provides a way to
        ///     separate trips by direction when publishing time tables.
        /// </summary>
        public bool Direction { get; set; }
    }
}
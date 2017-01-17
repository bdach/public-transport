using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     A StopTime defines when a vehicle arrives at a location, how long it stays there, and when it departs. StopTimes
    ///     define the path and schedule of <see cref="Entities.Trip" />s.
    /// </summary>
    public class StopTime : Entity
    {
        /// <summary>
        ///     Contains an ID that identifies a <see cref="Entities.Trip" />.
        /// </summary>
        [Required(ErrorMessage = "The stop time must be associated with a trip.")]
        public int TripId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Trip" /> this stop time is a part of.
        /// </summary>
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        /// <summary>
        ///     Contains an ID that uniquely identifies a <see cref="Entities.Stop" />.
        /// </summary>
        [Required(ErrorMessage = "The stop time must be associated with a stop.")]
        public int StopId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Stop" /> this stop time pertains to. Multiple routes may use the same stop. All stops
        ///     referenced here must have a <see cref="Entities.Stop.IsStation" /> value of <code>false</code>.
        /// </summary>
        [ForeignKey("StopId")]
        public Stop Stop { get; set; }

        /// <summary>
        ///     Specifies the arrival time at a specific stop for a specific trip on a route.
        /// </summary>
        [Required(ErrorMessage = "The arrival time is required.")]
        public TimeSpan ArrivalTime { get; set; }

        /// <summary>
        ///     Specifies the departure time at a specific stop for a specific trip on a route.
        /// </summary>
        [Required(ErrorMessage = "The departure time is required.")]
        public TimeSpan DepartureTime { get; set; }

        /// <summary>
        ///     Identifies the order of the stops for a particular trip. The values must be non-negative integers, and they must
        ///     increase along the trip.
        /// </summary>
        [Required(ErrorMessage = "The stop sequence number is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The stop sequence number must be non-negative.")]
        public int StopSequence { get; set; }

        /// <summary>
        ///     Associates the stop time with a <see cref="Entities.Shape"/>.
        /// </summary>
        public int? ShapeId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Shape"/> associated with the stop time.
        /// </summary>
        [ForeignKey("ShapeId")]
        public Shape Shape { get; set; }
    }
}
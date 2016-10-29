using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     A StopTime defines when a vehicle arrives at a location, how long it stays there, and when it departs. StopTimes
    ///     define the path and schedule of <see cref="Entities.Trip" />s.
    /// </summary>
    public class StopTime
    {
        /// <summary>
        ///     Contains an ID that identifies a <see cref="Entities.Trip" />.
        /// </summary>
        [Required]
        public int TripId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Trip" /> this stop time is a part of.
        /// </summary>
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        /// <summary>
        ///     Contains an ID that uniquely identifies a <see cref="Entities.Stop" />.
        /// </summary>
        [Required]
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
        [Required]
        public TimeSpan ArrivalTime { get; set; }

        /// <summary>
        ///     Specifies the departure time at a specific stop for a specific trip on a route.
        /// </summary>
        [Required]
        public TimeSpan DepartureTime { get; set; }

        /// <summary>
        ///     Identifies the order of the stops for a particular trip. The values must be non-negative integers, and they must
        ///     increase along the trip.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int StopSequence { get; set; }
    }
}
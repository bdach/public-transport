using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     A stop is a location where vehicles stop to pick up or drop off passengers. Stops can be grouped together, such as
    ///     when there are multiple stops within a single station. This is done by defining one Stop for the station, and
    ///     defining it as a parent for all the Stops it contains. Stops may also have <see cref="Entities.Zone" />
    ///     identifiers, to group them together into zones. This can be used together with <see cref="FareAttribute" />s and
    ///     <see cref="FareRule" />s for zone-based ticketing.
    /// </summary>
    public class Stop : Entity
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public Stop()
        {
            FavouritedBy = new List<User>();
        }

        /// <summary>
        ///     Contains the name of a stop or station. Please use a name that people will understand in the local and tourist
        ///     vernacular.
        /// </summary>
        [Required(ErrorMessage = "The stop name is required.")]
        public string Name { get; set; }

        /// <summary>
        ///     Contains an ID that uniquely identifies the <see cref="Entities.Street" /> the stop is located on.
        /// </summary>
        [Required(ErrorMessage = "The stop must be associated with a street.")]
        public int StreetId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Street" /> the stop is located on.
        /// </summary>
        [ForeignKey("StreetId")]
        public Street Street { get; set; }

        /// <summary>
        ///     Identifies the fare <see cref="Entities.Zone" /> for a stop ID. Zone IDs are required if you want to provide fare
        ///     information using <see cref="FareRule" />s. If this stop ID represents a station, the zone ID is ignored.
        /// </summary>
        public int? ZoneId { get; set; }

        /// <summary>
        ///     The <see cref="Entities.Zone" /> the stop is located in.
        /// </summary>
        [ForeignKey("ZoneId")]
        public Zone Zone { get; set; }

        /// <summary>
        ///     For stops that are physically located inside stations, this field identifies the station associated with the stop.
        ///     Used only if the value of <see cref="IsStation" /> is false.
        /// </summary>
        public int? ParentStationId { get; set; }

        /// <summary>
        ///     The station that contains this stop.
        /// </summary>
        [ForeignKey("ParentStationId")]
        public Stop ParentStation { get; set; }

        /// <summary>
        ///     Identifies whether this stop object represents a stop or station.
        /// </summary>
        [Required(ErrorMessage = "Please specify whether the stop is a station.")]
        public bool IsStation { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="User"/>s who favourited this stop.
        /// </summary>
        public IList<User> FavouritedBy { get; set; }
    }
}
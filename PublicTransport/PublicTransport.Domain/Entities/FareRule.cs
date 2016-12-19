using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Specifies how fares in <see cref="FareAttribute" /> apply to an itinerary.
    /// </summary>
    public class FareRule : Entity
    {
        /// <summary>
        ///     Associates the fare with a <see cref="Entities.Route" />.
        /// </summary>
        [Required(ErrorMessage = "The fare rule must be associated with a route.")]
        public int RouteId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Entities.Route" /> associated with the rule.
        /// </summary>
        [ForeignKey("RouteId")]
        public Route Route { get; set; }

        /// <summary>
        ///     Associates the fare with a origin zone ID.
        /// </summary>
        [Required(ErrorMessage = "The fare rule must have an origin zone.")]
        public int OriginId { get; set; }

        /// <summary>
        ///     Contains the origin <see cref="Zone" /> associated with the rule.
        /// </summary>
        [ForeignKey("OriginId")]
        public Zone Origin { get; set; }

        /// <summary>
        ///     Associates the fare with a destination zone ID.
        /// </summary>
        [Required(ErrorMessage = "The fare rule must have an destination zone.")]
        public int DestinationId { get; set; }

        /// <summary>
        ///     Contains the destination <see cref="Zone" /> associated with the rule.
        /// </summary>
        [ForeignKey("DestinationId")]
        public Zone Destination { get; set; }
    }
}
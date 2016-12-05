using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a zone containing multiple Stops. Zones affect fares in a way that is described in
    ///     <see cref="FareRule" /> objects.
    /// </summary>
    public class Zone : Entity
    {
        public Zone()
        {
            OriginFareRules = new List<FareRule>();
            DestinationFareRules = new List<FareRule>();
        }

        /// <summary>
        ///     Contains the name of the zone.
        /// </summary>
        [Required(ErrorMessage = "The zone name is required.")]
        public string Name { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="FareRule"/>s which have this zone referenced as origin.
        /// </summary>
        [InverseProperty("Origin")]
        public IList<FareRule> OriginFareRules { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="FareRule"/>s which have this zone referenced as destination.
        /// </summary>
        [InverseProperty("Destination")]
        public IList<FareRule> DestinationFareRules { get; set; }
    }
}
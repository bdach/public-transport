using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a zone containing multiple Stops. Zones affect fares in a way that is described in
    ///     <see cref="FareRule" /> objects.
    /// </summary>
    public class Zone : Entity
    {
        /// <summary>
        ///     Contains the name of the zone.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a street or other stretch of road within a metropolitan area.
    /// </summary>
    public class Street : Entity
    {
        /// <summary>
        ///     Contains an ID that uniquely identifies a city.
        /// </summary>
        [Required]
        public int CityId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Entities.City" /> that the street lies in.
        /// </summary>
        [ForeignKey("CityId")]
        public City City { get; set; }

        /// <summary>
        ///     Contains the name of the street.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
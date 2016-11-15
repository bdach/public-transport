using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents an operator of a public transit network.
    /// </summary>
    public class Agency : Entity
    {
        /// <summary>
        ///     The full name of the transit agency.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Contains a single voice telephone number for the specified agency.
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        ///     Contains the URL of the transit agency. The value must be a fully qualified URL that includes <code>http://</code>
        ///     or <code>https://</code>, and any special characters in the URL must be correctly escaped.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Contains the REGON number of the transit agency.
        /// </summary>
        public string Regon { get; set; }

        /// <summary>
        ///     Contains the ID number of the <see cref="Entities.Street" /> the agency is located on.
        /// </summary>
        public int? StreetId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Entities.Street" /> the agency is located on.
        /// </summary>
        [ForeignKey("StreetId")]
        public Street Street { get; set; }

        /// <summary>
        ///     Contains the street number of the agency.
        /// </summary>
        public string StreetNumber { get; set; }
    }
}
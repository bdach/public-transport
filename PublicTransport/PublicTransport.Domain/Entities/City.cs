using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a city or other starting/destination point.
    /// </summary>
    public class City : Entity
    {
        /// <summary>
        ///     Contains the name of the city.
        /// </summary>
        [Required(ErrorMessage = "The city name is required.")]
        public string Name { get; set; }
    }
}
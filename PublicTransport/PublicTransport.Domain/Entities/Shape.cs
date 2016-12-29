using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Shapes describe the physical path that a vehicle takes. Shapes are referenced by <see cref="StopTime" />s. Tracing
    ///     the points in order provides the path of the vehicle.
    /// </summary>
    public class Shape : Entity
    {
        /// <summary>
        ///     Associates a shape point's latitude with a shape ID. The field value must be a valid WGS 84 latitude.
        /// </summary>
        [Required]
        [Range(0, 90)]
        public decimal Latitude { get; set; }

        /// <summary>
        ///     Associates a shape point's longitude with a shape ID. The field value must be a valid WGS 84 longitude value from -180 to 180.
        /// </summary>
        [Required]
        [Range(-180, 180)]
        public decimal Longtitude { get; set; }

        /// <summary>
        ///     String used to identify the shape.
        /// </summary>
        public string Identifier { get; set; }
    }
}
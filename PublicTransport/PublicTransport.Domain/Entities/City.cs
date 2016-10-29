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
        public string Name { get; set; }
    }
}
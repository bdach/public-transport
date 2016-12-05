using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.City"/> objects.
    /// </summary>
    [DataContract]
    public class CityDto
    {
        // TODO: See if this can be private (probably not)
        /// <summary>
        /// City ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
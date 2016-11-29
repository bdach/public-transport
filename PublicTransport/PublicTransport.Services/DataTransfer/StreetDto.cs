using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// DTO for <see cref="Domain.Entities.Street"/> objects.
    /// </summary>
    [DataContract]
    public class StreetDto
    {
        /// <summary>
        /// Street ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Street name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// The city associated with the street.
        /// </summary>
        [DataMember]
        public CityDto City { get; set; }
    }
}
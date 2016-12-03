using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.Agency"/> objects.
    /// </summary>
    [DataContract]
    public class AgencyDto
    {
        /// <summary>
        /// Identification number of the city.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Agency name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Contains a single voice telephone number for the specified agency.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        ///     Contains the URL of the transit agency. The value must be a fully qualified URL that includes <code>http://</code>
        ///     or <code>https://</code>, and any special characters in the URL must be correctly escaped.
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        ///     Contains the REGON number of the transit agency.
        /// </summary>
        [DataMember]
        public string Regon { get; set; }

        /// <summary>
        /// Street associated with the agency.
        /// </summary>
        [DataMember]
        public StreetDto Street { get; set; }

        /// <summary>
        ///     Contains the street number of the agency.
        /// </summary>
        [DataMember]
        public string StreetNumber { get; set; }
    }
}

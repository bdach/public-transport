using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class ZoneDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
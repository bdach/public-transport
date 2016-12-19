using System.Runtime.Serialization;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class RouteDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public AgencyDto Agency { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string LongName { get; set; }
        [DataMember]
        public RouteType RouteType { get; set; }
    }
}
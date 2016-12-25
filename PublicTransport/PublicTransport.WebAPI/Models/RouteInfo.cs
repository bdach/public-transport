using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using System.Runtime.Serialization;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class RouteInfo
    {
        public RouteInfo(Route route)
        {
            Id = route.Id;
            ShortName = route.ShortName;
            LongName = route.LongName;
            RouteType = route.RouteType;
            Agency = new AgencyInfo(route.Agency);
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string LongName { get; set; }
        [DataMember]
        public RouteType RouteType { get; set; }
        [DataMember]
        public AgencyInfo Agency { get; set; }
    }
}
using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class TripDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public RouteDto Route { get; set; }
        [DataMember]
        public CalendarDto Service { get; set; }
        [DataMember]
        public string Headsign { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public bool Direction { get; set; }
    }
}
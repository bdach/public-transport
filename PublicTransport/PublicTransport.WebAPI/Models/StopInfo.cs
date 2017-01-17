using PublicTransport.Domain.Entities;
using System.Runtime.Serialization;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class StopInfo
    {
        public StopInfo(Stop stop)
        {
            Id = stop.Id;
            Name = stop.Name;
            StreetName = stop.Street?.Name;
            CityName = stop.Street?.City?.Name;
            ParentStation = stop.ParentStation == null ? null : new StopInfo(stop.ParentStation);
            IsStation = stop.IsStation;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string StreetName { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public StopInfo ParentStation { get; set; }
        [DataMember]
        public bool IsStation { get; set; }
    }
}
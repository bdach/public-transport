using System;
using System.Runtime.Serialization;
using PublicTransport.Domain.Entities;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class TripInfo
    {
        public TripInfo(Tuple<Trip, StopTime, StopTime> tuple)
        {
            Id = tuple.Item1.Id;
            OriginStop = new TripStop(tuple.Item2);
            DestinationStop = new TripStop(tuple.Item3);
            Route = new RouteInfo(tuple.Item1.Route);
            ServiceDetails = new ServiceInfo(tuple.Item1.Service);
        }
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public TripStop OriginStop { get; set; }
        [DataMember]
        public TripStop DestinationStop { get; set; }
        [DataMember]
        public RouteInfo Route { get; set; }
        [DataMember]
        public ServiceInfo ServiceDetails { get; set; }
    }
}
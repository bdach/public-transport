using PublicTransport.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class StopTimeInfo
    {
        public StopTimeInfo(StopTime stopTime)
        {
            ArrivalTime = stopTime.ArrivalTime;
            DepartureTime = stopTime.DepartureTime;
            ShortName = stopTime.Trip.ShortName;
            Headsign = stopTime.Trip.Headsign;
            Direction = stopTime.Trip.Direction;
        }

        [DataMember]
        public TimeSpan ArrivalTime { get; set; }
        [DataMember]
        public TimeSpan DepartureTime { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Headsign { get; set; }
        [DataMember]
        public bool Direction { get; set; }
    }
}
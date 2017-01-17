using System;
using System.Runtime.Serialization;
using PublicTransport.Domain.Entities;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class MapMarker
    {
        public MapMarker(StopTime stopTime)
        {
            Latitude = stopTime.Shape.Latitude;
            Longtitude = stopTime.Shape.Longtitude;
            DescriptionText = stopTime.Shape.Identifier ?? stopTime.Stop.Name;
            ArrivalTime = stopTime.ArrivalTime;
            DepartureTime = stopTime.DepartureTime;
        }

        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longtitude { get; set; }
        [DataMember]
        public string DescriptionText { get; set; }
        [DataMember]
        public TimeSpan ArrivalTime { get; set; }
        [DataMember]
        public TimeSpan DepartureTime { get; set; }
    }
}
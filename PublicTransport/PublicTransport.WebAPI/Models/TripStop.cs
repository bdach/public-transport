using System;
using System.Runtime.Serialization;
using PublicTransport.Domain.Entities;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class TripStop
    {
        public TripStop(StopTime stopTime)
        {
            Stop = new StopInfo(stopTime.Stop);
            ArrivalTime = stopTime.ArrivalTime;
            DepartureTime = stopTime.DepartureTime;
            SequenceNumber = stopTime.StopSequence;
        }

        [DataMember]
        public StopInfo Stop { get; set; }
        [DataMember]
        public TimeSpan ArrivalTime { get; set; }
        [DataMember]
        public TimeSpan DepartureTime { get; set; }
        [DataMember]
        public int SequenceNumber { get; set; }
    }
}
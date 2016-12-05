using System;
using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class StopTimeDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public TripDto Trip { get; set; }
        [DataMember]
        public StopDto Stop { get; set; }
        [DataMember]
        public TimeSpan ArrivalTime { get; set; }
        [DataMember]
        public TimeSpan DepartureTime { get; set; }
        [DataMember]
        public int StopSequence { get; set; }
    }
}
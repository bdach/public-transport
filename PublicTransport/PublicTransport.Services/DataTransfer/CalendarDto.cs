using System;
using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class CalendarDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool Monday { get; set; }
        [DataMember]
        public bool Tuesday { get; set; }
        [DataMember]
        public bool Wednesday { get; set; }
        [DataMember]
        public bool Thursday { get; set; }
        [DataMember]
        public bool Friday { get; set; }
        [DataMember]
        public bool Saturday { get; set; }
        [DataMember]
        public bool Sunday { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
using System;
using System.Runtime.Serialization;
using PublicTransport.Domain.Entities;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class ServiceInfo
    {
        public ServiceInfo(Calendar calendar)
        {
            Monday = calendar.Monday;
            Tuesday = calendar.Tuesday;
            Wednesday = calendar.Wednesday;
            Thursday = calendar.Thursday;
            Friday = calendar.Friday;
            Saturday = calendar.Saturday;
            Sunday = calendar.Sunday;
            StartDate = calendar.StartDate;
            EndDate = calendar.EndDate;
        }

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
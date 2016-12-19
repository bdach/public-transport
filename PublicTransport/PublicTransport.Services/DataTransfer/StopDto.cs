using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    [DataContract]
    public class StopDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public StreetDto Street { get; set; }
        [DataMember]
        public ZoneDto Zone { get; set; }
        [DataMember]
        public StopDto ParentStation { get; set; }
        [DataMember]
        public bool IsStation { get; set; }
    }
}
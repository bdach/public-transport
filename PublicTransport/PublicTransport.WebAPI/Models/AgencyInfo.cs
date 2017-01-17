using PublicTransport.Domain.Entities;
using System.Runtime.Serialization;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class AgencyInfo
    {
        public AgencyInfo(Agency agency)
        {
            Name = agency.Name;
            Phone = agency.Phone;
            Url = agency.Url;
            Regon = agency.Regon;
            StreetName = agency.Street?.Name;
            StreetNumber = agency.StreetNumber;
            CityName = agency.Street?.City?.Name;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Regon { get; set; }
        [DataMember]
        public string StreetName { get; set; }
        [DataMember]
        public string StreetNumber { get; set; }
        [DataMember]
        public string CityName { get; set; }

        public static AgencyInfo Convert(Agency agency)
        {
            return agency == null ? null : new AgencyInfo(agency);
        }
    }
}
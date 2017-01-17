using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PublicTransport.WebAPI.Models
{
    [DataContract]
    public class FavouriteInfo
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public Dictionary<int, bool> Changes { get; set; }
    }
}
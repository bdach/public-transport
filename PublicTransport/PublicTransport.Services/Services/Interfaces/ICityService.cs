using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Services.Interfaces
{
    [ServiceContract]
    public interface ICityService
    {
        [OperationContract]
        CityDto CreateCity(CityDto city);
        
        [OperationContract]
        CityDto UpdateCity(CityDto city);

        [OperationContract]
        void DeleteCity(CityDto city);

        [OperationContract]
        List<CityDto> FilterCities(string str);
    }
}
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Interfaces
{
    [ServiceContract]
    public interface ICityService
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto CreateCity(CityDto city);
        
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto UpdateCity(CityDto city);

        [OperationContract]
        void DeleteCity(CityDto city);

        [OperationContract]
        List<CityDto> FilterCities(string str);
    }
}
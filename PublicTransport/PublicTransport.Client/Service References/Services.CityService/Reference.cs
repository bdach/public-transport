﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublicTransport.Client.Services.CityService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.CityService.ICityService")]
    public interface ICityService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/CreateCity", ReplyAction="http://tempuri.org/ICityService/CreateCityResponse")]
        PublicTransport.Services.DataTransfer.CityDto CreateCity(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/CreateCity", ReplyAction="http://tempuri.org/ICityService/CreateCityResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto> CreateCityAsync(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/UpdateCity", ReplyAction="http://tempuri.org/ICityService/UpdateCityResponse")]
        PublicTransport.Services.DataTransfer.CityDto UpdateCity(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/UpdateCity", ReplyAction="http://tempuri.org/ICityService/UpdateCityResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto> UpdateCityAsync(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/DeleteCity", ReplyAction="http://tempuri.org/ICityService/DeleteCityResponse")]
        void DeleteCity(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/DeleteCity", ReplyAction="http://tempuri.org/ICityService/DeleteCityResponse")]
        System.Threading.Tasks.Task DeleteCityAsync(PublicTransport.Services.DataTransfer.CityDto city);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/FilterCities", ReplyAction="http://tempuri.org/ICityService/FilterCitiesResponse")]
        PublicTransport.Services.DataTransfer.CityDto[] FilterCities(string str);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICityService/FilterCities", ReplyAction="http://tempuri.org/ICityService/FilterCitiesResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto[]> FilterCitiesAsync(string str);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICityServiceChannel : PublicTransport.Client.Services.CityService.ICityService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CityServiceClient : System.ServiceModel.ClientBase<PublicTransport.Client.Services.CityService.ICityService>, PublicTransport.Client.Services.CityService.ICityService {
        
        public CityServiceClient() {
        }
        
        public CityServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CityServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CityServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CityServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PublicTransport.Services.DataTransfer.CityDto CreateCity(PublicTransport.Services.DataTransfer.CityDto city) {
            return base.Channel.CreateCity(city);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto> CreateCityAsync(PublicTransport.Services.DataTransfer.CityDto city) {
            return base.Channel.CreateCityAsync(city);
        }
        
        public PublicTransport.Services.DataTransfer.CityDto UpdateCity(PublicTransport.Services.DataTransfer.CityDto city) {
            return base.Channel.UpdateCity(city);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto> UpdateCityAsync(PublicTransport.Services.DataTransfer.CityDto city) {
            return base.Channel.UpdateCityAsync(city);
        }
        
        public void DeleteCity(PublicTransport.Services.DataTransfer.CityDto city) {
            base.Channel.DeleteCity(city);
        }
        
        public System.Threading.Tasks.Task DeleteCityAsync(PublicTransport.Services.DataTransfer.CityDto city) {
            return base.Channel.DeleteCityAsync(city);
        }
        
        public PublicTransport.Services.DataTransfer.CityDto[] FilterCities(string str) {
            return base.Channel.FilterCities(str);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto[]> FilterCitiesAsync(string str) {
            return base.Channel.FilterCitiesAsync(str);
        }
    }
}

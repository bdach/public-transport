﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublicTransport.Client.Services.Streets {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.Streets.IStreetService")]
    public interface IStreetService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/CreateStreet", ReplyAction="http://tempuri.org/IStreetService/CreateStreetResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IStreetService/CreateStreetValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.StreetDto CreateStreet(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/CreateStreet", ReplyAction="http://tempuri.org/IStreetService/CreateStreetResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto> CreateStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/UpdateStreet", ReplyAction="http://tempuri.org/IStreetService/UpdateStreetResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IStreetService/UpdateStreetValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.StreetDto UpdateStreet(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/UpdateStreet", ReplyAction="http://tempuri.org/IStreetService/UpdateStreetResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto> UpdateStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/DeleteStreet", ReplyAction="http://tempuri.org/IStreetService/DeleteStreetResponse")]
        void DeleteStreet(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/DeleteStreet", ReplyAction="http://tempuri.org/IStreetService/DeleteStreetResponse")]
        System.Threading.Tasks.Task DeleteStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/FilterCities", ReplyAction="http://tempuri.org/IStreetService/FilterCitiesResponse")]
        PublicTransport.Services.DataTransfer.CityDto[] FilterCities(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/FilterCities", ReplyAction="http://tempuri.org/IStreetService/FilterCitiesResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto[]> FilterCitiesAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/FilterStreets", ReplyAction="http://tempuri.org/IStreetService/FilterStreetsResponse")]
        PublicTransport.Services.DataTransfer.StreetDto[] FilterStreets(PublicTransport.Services.DataTransfer.Filters.StreetFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreetService/FilterStreets", ReplyAction="http://tempuri.org/IStreetService/FilterStreetsResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto[]> FilterStreetsAsync(PublicTransport.Services.DataTransfer.Filters.StreetFilter filter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStreetServiceChannel : PublicTransport.Client.Services.Streets.IStreetService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StreetServiceClient : System.ServiceModel.ClientBase<PublicTransport.Client.Services.Streets.IStreetService>, PublicTransport.Client.Services.Streets.IStreetService {
        
        public StreetServiceClient() {
        }
        
        public StreetServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StreetServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreetServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreetServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PublicTransport.Services.DataTransfer.StreetDto CreateStreet(PublicTransport.Services.DataTransfer.StreetDto street) {
            return base.Channel.CreateStreet(street);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto> CreateStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street) {
            return base.Channel.CreateStreetAsync(street);
        }
        
        public PublicTransport.Services.DataTransfer.StreetDto UpdateStreet(PublicTransport.Services.DataTransfer.StreetDto street) {
            return base.Channel.UpdateStreet(street);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto> UpdateStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street) {
            return base.Channel.UpdateStreetAsync(street);
        }
        
        public void DeleteStreet(PublicTransport.Services.DataTransfer.StreetDto street) {
            base.Channel.DeleteStreet(street);
        }
        
        public System.Threading.Tasks.Task DeleteStreetAsync(PublicTransport.Services.DataTransfer.StreetDto street) {
            return base.Channel.DeleteStreetAsync(street);
        }
        
        public PublicTransport.Services.DataTransfer.CityDto[] FilterCities(string name) {
            return base.Channel.FilterCities(name);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CityDto[]> FilterCitiesAsync(string name) {
            return base.Channel.FilterCitiesAsync(name);
        }
        
        public PublicTransport.Services.DataTransfer.StreetDto[] FilterStreets(PublicTransport.Services.DataTransfer.Filters.StreetFilter filter) {
            return base.Channel.FilterStreets(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StreetDto[]> FilterStreetsAsync(PublicTransport.Services.DataTransfer.Filters.StreetFilter filter) {
            return base.Channel.FilterStreetsAsync(filter);
        }
    }
}

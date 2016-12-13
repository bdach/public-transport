﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublicTransport.Client.Services.Search {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.Search.ISearchService")]
    public interface ISearchService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISearchService/FilterStops", ReplyAction="http://tempuri.org/ISearchService/FilterStopsResponse")]
        PublicTransport.Services.DataTransfer.StopDto[] FilterStops(PublicTransport.Services.DataTransfer.Filters.StopFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISearchService/FilterStops", ReplyAction="http://tempuri.org/ISearchService/FilterStopsResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> FilterStopsAsync(PublicTransport.Services.DataTransfer.Filters.StopFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISearchService/FindRoutes", ReplyAction="http://tempuri.org/ISearchService/FindRoutesResponse")]
        PublicTransport.Services.DataTransfer.RouteDto[] FindRoutes(PublicTransport.Services.DataTransfer.Filters.RouteSearchFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISearchService/FindRoutes", ReplyAction="http://tempuri.org/ISearchService/FindRoutesResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto[]> FindRoutesAsync(PublicTransport.Services.DataTransfer.Filters.RouteSearchFilter filter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISearchServiceChannel : PublicTransport.Client.Services.Search.ISearchService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SearchServiceClient : System.ServiceModel.ClientBase<PublicTransport.Client.Services.Search.ISearchService>, PublicTransport.Client.Services.Search.ISearchService {
        
        public SearchServiceClient() {
        }
        
        public SearchServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SearchServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SearchServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SearchServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PublicTransport.Services.DataTransfer.StopDto[] FilterStops(PublicTransport.Services.DataTransfer.Filters.StopFilter filter) {
            return base.Channel.FilterStops(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> FilterStopsAsync(PublicTransport.Services.DataTransfer.Filters.StopFilter filter) {
            return base.Channel.FilterStopsAsync(filter);
        }
        
        public PublicTransport.Services.DataTransfer.RouteDto[] FindRoutes(PublicTransport.Services.DataTransfer.Filters.RouteSearchFilter filter) {
            return base.Channel.FindRoutes(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto[]> FindRoutesAsync(PublicTransport.Services.DataTransfer.Filters.RouteSearchFilter filter) {
            return base.Channel.FindRoutesAsync(filter);
        }
    }
}

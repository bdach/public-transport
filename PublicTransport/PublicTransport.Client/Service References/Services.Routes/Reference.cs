﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublicTransport.Client.Services.Routes {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.Routes.IRouteService")]
    public interface IRouteService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetRouteTimetableByStopId", ReplyAction="http://tempuri.org/IRouteService/GetRouteTimetableByStopIdResponse")]
        PublicTransport.Services.DataTransfer.StopTimeDto[] GetRouteTimetableByStopId(PublicTransport.Services.DataTransfer.Filters.StopTimeFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetRouteTimetableByStopId", ReplyAction="http://tempuri.org/IRouteService/GetRouteTimetableByStopIdResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> GetRouteTimetableByStopIdAsync(PublicTransport.Services.DataTransfer.Filters.StopTimeFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateCalendar", ReplyAction="http://tempuri.org/IRouteService/CreateCalendarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/CreateCalendarValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.CalendarDto CreateCalendar(PublicTransport.Services.DataTransfer.CalendarDto calendar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateCalendar", ReplyAction="http://tempuri.org/IRouteService/CreateCalendarResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CalendarDto> CreateCalendarAsync(PublicTransport.Services.DataTransfer.CalendarDto calendar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateCalendar", ReplyAction="http://tempuri.org/IRouteService/UpdateCalendarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/UpdateCalendarValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.CalendarDto UpdateCalendar(PublicTransport.Services.DataTransfer.CalendarDto calendar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateCalendar", ReplyAction="http://tempuri.org/IRouteService/UpdateCalendarResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CalendarDto> UpdateCalendarAsync(PublicTransport.Services.DataTransfer.CalendarDto calendar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterAgencies", ReplyAction="http://tempuri.org/IRouteService/FilterAgenciesResponse")]
        PublicTransport.Services.DataTransfer.AgencyDto[] FilterAgencies(PublicTransport.Services.DataTransfer.Filters.AgencyFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterAgencies", ReplyAction="http://tempuri.org/IRouteService/FilterAgenciesResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.AgencyDto[]> FilterAgenciesAsync(PublicTransport.Services.DataTransfer.Filters.AgencyFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateRoute", ReplyAction="http://tempuri.org/IRouteService/CreateRouteResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/CreateRouteValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.RouteDto CreateRoute(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateRoute", ReplyAction="http://tempuri.org/IRouteService/CreateRouteResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto> CreateRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateRoute", ReplyAction="http://tempuri.org/IRouteService/UpdateRouteResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/UpdateRouteValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.RouteDto UpdateRoute(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateRoute", ReplyAction="http://tempuri.org/IRouteService/UpdateRouteResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto> UpdateRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/DeleteRoute", ReplyAction="http://tempuri.org/IRouteService/DeleteRouteResponse")]
        void DeleteRoute(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/DeleteRoute", ReplyAction="http://tempuri.org/IRouteService/DeleteRouteResponse")]
        System.Threading.Tasks.Task DeleteRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterRoutes", ReplyAction="http://tempuri.org/IRouteService/FilterRoutesResponse")]
        PublicTransport.Services.DataTransfer.RouteDto[] FilterRoutes(PublicTransport.Services.DataTransfer.Filters.RouteFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterRoutes", ReplyAction="http://tempuri.org/IRouteService/FilterRoutesResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto[]> FilterRoutesAsync(PublicTransport.Services.DataTransfer.Filters.RouteFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterStops", ReplyAction="http://tempuri.org/IRouteService/FilterStopsResponse")]
        PublicTransport.Services.DataTransfer.StopDto[] FilterStops(PublicTransport.Services.DataTransfer.Filters.StopFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/FilterStops", ReplyAction="http://tempuri.org/IRouteService/FilterStopsResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> FilterStopsAsync(PublicTransport.Services.DataTransfer.Filters.StopFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetStopsByRouteId", ReplyAction="http://tempuri.org/IRouteService/GetStopsByRouteIdResponse")]
        PublicTransport.Services.DataTransfer.StopDto[] GetStopsByRouteId(int routeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetStopsByRouteId", ReplyAction="http://tempuri.org/IRouteService/GetStopsByRouteIdResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> GetStopsByRouteIdAsync(int routeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateTrip", ReplyAction="http://tempuri.org/IRouteService/CreateTripResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/CreateTripValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.TripDto CreateTrip(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/CreateTrip", ReplyAction="http://tempuri.org/IRouteService/CreateTripResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.TripDto> CreateTripAsync(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateTrip", ReplyAction="http://tempuri.org/IRouteService/UpdateTripResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/UpdateTripValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.TripDto UpdateTrip(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateTrip", ReplyAction="http://tempuri.org/IRouteService/UpdateTripResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.TripDto> UpdateTripAsync(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/DeleteTrip", ReplyAction="http://tempuri.org/IRouteService/DeleteTripResponse")]
        void DeleteTrip(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/DeleteTrip", ReplyAction="http://tempuri.org/IRouteService/DeleteTripResponse")]
        System.Threading.Tasks.Task DeleteTripAsync(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateStops", ReplyAction="http://tempuri.org/IRouteService/UpdateStopsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(PublicTransport.Services.Exceptions.ValidationFault), Action="http://tempuri.org/IRouteService/UpdateStopsValidationFaultFault", Name="ValidationFault", Namespace="http://schemas.datacontract.org/2004/07/PublicTransport.Services.Exceptions")]
        PublicTransport.Services.DataTransfer.StopTimeDto[] UpdateStops(int tripId, PublicTransport.Services.DataTransfer.StopTimeDto[] stops);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/UpdateStops", ReplyAction="http://tempuri.org/IRouteService/UpdateStopsResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> UpdateStopsAsync(int tripId, PublicTransport.Services.DataTransfer.StopTimeDto[] stops);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetTripStops", ReplyAction="http://tempuri.org/IRouteService/GetTripStopsResponse")]
        PublicTransport.Services.DataTransfer.StopTimeDto[] GetTripStops(PublicTransport.Services.DataTransfer.TripDto trip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRouteService/GetTripStops", ReplyAction="http://tempuri.org/IRouteService/GetTripStopsResponse")]
        System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> GetTripStopsAsync(PublicTransport.Services.DataTransfer.TripDto trip);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRouteServiceChannel : PublicTransport.Client.Services.Routes.IRouteService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RouteServiceClient : System.ServiceModel.ClientBase<PublicTransport.Client.Services.Routes.IRouteService>, PublicTransport.Client.Services.Routes.IRouteService {
        
        public RouteServiceClient() {
        }
        
        public RouteServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RouteServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RouteServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RouteServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PublicTransport.Services.DataTransfer.StopTimeDto[] GetRouteTimetableByStopId(PublicTransport.Services.DataTransfer.Filters.StopTimeFilter filter) {
            return base.Channel.GetRouteTimetableByStopId(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> GetRouteTimetableByStopIdAsync(PublicTransport.Services.DataTransfer.Filters.StopTimeFilter filter) {
            return base.Channel.GetRouteTimetableByStopIdAsync(filter);
        }
        
        public PublicTransport.Services.DataTransfer.CalendarDto CreateCalendar(PublicTransport.Services.DataTransfer.CalendarDto calendar) {
            return base.Channel.CreateCalendar(calendar);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CalendarDto> CreateCalendarAsync(PublicTransport.Services.DataTransfer.CalendarDto calendar) {
            return base.Channel.CreateCalendarAsync(calendar);
        }
        
        public PublicTransport.Services.DataTransfer.CalendarDto UpdateCalendar(PublicTransport.Services.DataTransfer.CalendarDto calendar) {
            return base.Channel.UpdateCalendar(calendar);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.CalendarDto> UpdateCalendarAsync(PublicTransport.Services.DataTransfer.CalendarDto calendar) {
            return base.Channel.UpdateCalendarAsync(calendar);
        }
        
        public PublicTransport.Services.DataTransfer.AgencyDto[] FilterAgencies(PublicTransport.Services.DataTransfer.Filters.AgencyFilter filter) {
            return base.Channel.FilterAgencies(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.AgencyDto[]> FilterAgenciesAsync(PublicTransport.Services.DataTransfer.Filters.AgencyFilter filter) {
            return base.Channel.FilterAgenciesAsync(filter);
        }
        
        public PublicTransport.Services.DataTransfer.RouteDto CreateRoute(PublicTransport.Services.DataTransfer.RouteDto route) {
            return base.Channel.CreateRoute(route);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto> CreateRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route) {
            return base.Channel.CreateRouteAsync(route);
        }
        
        public PublicTransport.Services.DataTransfer.RouteDto UpdateRoute(PublicTransport.Services.DataTransfer.RouteDto route) {
            return base.Channel.UpdateRoute(route);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto> UpdateRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route) {
            return base.Channel.UpdateRouteAsync(route);
        }
        
        public void DeleteRoute(PublicTransport.Services.DataTransfer.RouteDto route) {
            base.Channel.DeleteRoute(route);
        }
        
        public System.Threading.Tasks.Task DeleteRouteAsync(PublicTransport.Services.DataTransfer.RouteDto route) {
            return base.Channel.DeleteRouteAsync(route);
        }
        
        public PublicTransport.Services.DataTransfer.RouteDto[] FilterRoutes(PublicTransport.Services.DataTransfer.Filters.RouteFilter filter) {
            return base.Channel.FilterRoutes(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.RouteDto[]> FilterRoutesAsync(PublicTransport.Services.DataTransfer.Filters.RouteFilter filter) {
            return base.Channel.FilterRoutesAsync(filter);
        }
        
        public PublicTransport.Services.DataTransfer.StopDto[] FilterStops(PublicTransport.Services.DataTransfer.Filters.StopFilter filter) {
            return base.Channel.FilterStops(filter);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> FilterStopsAsync(PublicTransport.Services.DataTransfer.Filters.StopFilter filter) {
            return base.Channel.FilterStopsAsync(filter);
        }
        
        public PublicTransport.Services.DataTransfer.StopDto[] GetStopsByRouteId(int routeId) {
            return base.Channel.GetStopsByRouteId(routeId);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopDto[]> GetStopsByRouteIdAsync(int routeId) {
            return base.Channel.GetStopsByRouteIdAsync(routeId);
        }
        
        public PublicTransport.Services.DataTransfer.TripDto CreateTrip(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.CreateTrip(trip);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.TripDto> CreateTripAsync(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.CreateTripAsync(trip);
        }
        
        public PublicTransport.Services.DataTransfer.TripDto UpdateTrip(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.UpdateTrip(trip);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.TripDto> UpdateTripAsync(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.UpdateTripAsync(trip);
        }
        
        public void DeleteTrip(PublicTransport.Services.DataTransfer.TripDto trip) {
            base.Channel.DeleteTrip(trip);
        }
        
        public System.Threading.Tasks.Task DeleteTripAsync(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.DeleteTripAsync(trip);
        }
        
        public PublicTransport.Services.DataTransfer.StopTimeDto[] UpdateStops(int tripId, PublicTransport.Services.DataTransfer.StopTimeDto[] stops) {
            return base.Channel.UpdateStops(tripId, stops);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> UpdateStopsAsync(int tripId, PublicTransport.Services.DataTransfer.StopTimeDto[] stops) {
            return base.Channel.UpdateStopsAsync(tripId, stops);
        }
        
        public PublicTransport.Services.DataTransfer.StopTimeDto[] GetTripStops(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.GetTripStops(trip);
        }
        
        public System.Threading.Tasks.Task<PublicTransport.Services.DataTransfer.StopTimeDto[]> GetTripStopsAsync(PublicTransport.Services.DataTransfer.TripDto trip) {
            return base.Channel.GetTripStopsAsync(trip);
        }
    }
}

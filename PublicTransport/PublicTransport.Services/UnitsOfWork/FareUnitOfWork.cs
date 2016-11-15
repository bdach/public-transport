using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    public interface IFareUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        FareAttribute CreateFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> update method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        FareAttribute UpdateFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        void DeleteFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        FareRule CreateFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> update method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        FareRule UpdateFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        void DeleteFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        List<FareAttribute> FilterFares(IFareFilter filter);

        /// <summary>
        ///     Calls <see cref="RouteService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        List<Route> FilterRoutes(IRouteFilter filter);

        /// <summary>
        ///     Calls <see cref="ZoneService"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        List<Zone> FilterZones(string name);
    }

    /// <summary>
    ///     Unit of work used to manage fare data.
    /// </summary>
    public class FareUnitOfWork : IFareUnitOfWork
    {
        /// <summary>
        ///     Service used to fetch <see cref="FareAttribute"/> data from the database.
        /// </summary>
        private readonly FareAttributeService _fareAttributeService;

        /// <summary>
        ///     Service used to fetch <see cref="FareRule"/> data from the database.
        /// </summary>
        private readonly FareRuleService _fareRuleService;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteService _routeService;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneService _zoneService;

        /// <summary>
        ///     Database context common for services in this unit of work used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public FareUnitOfWork()
        {
            _db = new PublicTransportContext();
            _fareAttributeService = new FareAttributeService(_db);
            _fareRuleService = new FareRuleService(_db);
            _routeService = new RouteService(_db);
            _zoneService = new ZoneService(_db);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        public FareAttribute CreateFareAttribute(FareAttribute fareAttribute)
        {
            return _fareAttributeService.Create(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> update method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public FareAttribute UpdateFareAttribute(FareAttribute fareAttribute)
        {
            return _fareAttributeService.Update(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public void DeleteFareAttribute(FareAttribute fareAttribute)
        {
            _fareAttributeService.Delete(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        public FareRule CreateFareRule(FareRule fareRule)
        {
            return _fareRuleService.Create(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> update method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public FareRule UpdateFareRule(FareRule fareRule)
        {
            return _fareRuleService.Update(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleService"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public void DeleteFareRule(FareRule fareRule)
        {
            _fareRuleService.Delete(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        public List<FareAttribute> FilterFares(IFareFilter filter)
        {
            return _fareAttributeService.FilterFares(filter);
        }

        /// <summary>
        ///     Calls <see cref="RouteService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        public List<Route> FilterRoutes(IRouteFilter filter)
        {
            return _routeService.FilterRoutes(filter);
        }

        /// <summary>
        ///     Calls <see cref="ZoneService"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<Zone> FilterZones(string name)
        {
            return _zoneService.GetZonesContainingString(name);
        }

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}
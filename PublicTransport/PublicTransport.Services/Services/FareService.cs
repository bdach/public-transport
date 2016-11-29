using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    public interface IFareService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        FareAttribute CreateFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> update method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        FareAttribute UpdateFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        void DeleteFareAttribute(FareAttribute fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        FareRule CreateFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> update method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        FareRule UpdateFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        void DeleteFareRule(FareRule fareRule);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        List<FareAttribute> FilterFares(IFareFilter filter);

        /// <summary>
        ///     Calls <see cref="RouteRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        List<Route> FilterRoutes(IRouteFilter filter);

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
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
    public class FareService : IFareService
    {
        /// <summary>
        ///     Service used to fetch <see cref="FareAttribute"/> data from the database.
        /// </summary>
        private readonly FareAttributeRepository _fareAttributeRepository;

        /// <summary>
        ///     Service used to fetch <see cref="FareRule"/> data from the database.
        /// </summary>
        private readonly FareRuleRepository _fareRuleRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteRepository _routeRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneRepository _zoneRepository;

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
        public FareService()
        {
            _db = new PublicTransportContext();
            _fareAttributeRepository = new FareAttributeRepository(_db);
            _fareRuleRepository = new FareRuleRepository(_db);
            _routeRepository = new RouteRepository(_db);
            _zoneRepository = new ZoneRepository(_db);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        public FareAttribute CreateFareAttribute(FareAttribute fareAttribute)
        {
            return _fareAttributeRepository.Create(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> update method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public FareAttribute UpdateFareAttribute(FareAttribute fareAttribute)
        {
            return _fareAttributeRepository.Update(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public void DeleteFareAttribute(FareAttribute fareAttribute)
        {
            _fareAttributeRepository.Delete(fareAttribute);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        public FareRule CreateFareRule(FareRule fareRule)
        {
            return _fareRuleRepository.Create(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> update method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public FareRule UpdateFareRule(FareRule fareRule)
        {
            return _fareRuleRepository.Update(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public void DeleteFareRule(FareRule fareRule)
        {
            _fareRuleRepository.Delete(fareRule);
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        public List<FareAttribute> FilterFares(IFareFilter filter)
        {
            return _fareAttributeRepository.FilterFares(filter);
        }

        /// <summary>
        ///     Calls <see cref="RouteRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        public List<Route> FilterRoutes(IRouteFilter filter)
        {
            return _routeRepository.FilterRoutes(filter);
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<Zone> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name);
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
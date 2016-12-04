using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage fare data.
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
        ///     Used for converting <see cref="Domain.Entities.FareAttribute" /> objects to <see cref="FareAttributeDto" /> objects and back.
        /// </summary>
        private readonly IConverter<FareAttribute, FareAttributeDto> _fareAttributeConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.FareRule" /> objects to <see cref="FareRuleDto" /> objects and back.
        /// </summary>
        private readonly IConverter<FareRule, FareRuleDto> _fareRuleConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Route" /> objects to <see cref="RouteDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Route, RouteDto> _routeConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _zoneConverter;

        /// <summary>
        ///     Database context common for services in this service used to access data.
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
            _fareAttributeConverter = new FareAttributeConverter();
            _fareRuleConverter = new FareRuleConverter();
            _routeConverter = new RouteConverter();
            _zoneConverter = new ZoneConverter();
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        public FareAttributeDto CreateFareAttribute(FareAttributeDto fareAttribute)
        {
            try
            {
                var result = _fareAttributeRepository.Create(_fareAttributeConverter.GetEntity(fareAttribute));
                return _fareAttributeConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
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
        public FareAttributeDto UpdateFareAttribute(FareAttributeDto fareAttribute)
        {
            try
            {
                var result = _fareAttributeRepository.Update(_fareAttributeConverter.GetEntity(fareAttribute));
                return _fareAttributeConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public void DeleteFareAttribute(FareAttributeDto fareAttribute)
        {
            _fareAttributeRepository.Delete(_fareAttributeConverter.GetEntity(fareAttribute));
        }

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        public FareRuleDto CreateFareRule(FareRuleDto fareRule)
        {
            try
            {
                var result = _fareRuleRepository.Create(_fareRuleConverter.GetEntity(fareRule));
                return _fareRuleConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
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
        public FareRuleDto UpdateFareRule(FareRuleDto fareRule)
        {
            try
            {
                var result = _fareRuleRepository.Update(_fareRuleConverter.GetEntity(fareRule));
                return _fareRuleConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public void DeleteFareRule(FareRuleDto fareRule)
        {
            _fareRuleRepository.Delete(_fareRuleConverter.GetEntity(fareRule));
        }

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        public List<FareAttributeDto> FilterFares(FareFilter filter)
        {
            return _fareAttributeRepository.FilterFares(filter)
                .Select(_fareAttributeConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Calls <see cref="RouteRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        public List<RouteDto> FilterRoutes(RouteFilter filter)
        {
            return _routeRepository.FilterRoutes(filter)
                .Select(_routeConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<ZoneDto> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name)
                .Select(_zoneConverter.GetDto)
                .ToList();
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
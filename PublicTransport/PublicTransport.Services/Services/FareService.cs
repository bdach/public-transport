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
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Used for converting <see cref="FareAttribute" /> objects to <see cref="FareAttributeDto" /> objects and back.
        /// </summary>
        private readonly IConverter<FareAttribute, FareAttributeDto> _fareAttributeConverter;

        /// <summary>
        ///     Service used to fetch <see cref="FareAttribute" /> data from the database.
        /// </summary>
        private readonly FareAttributeRepository _fareAttributeRepository;

        /// <summary>
        ///     Used for converting <see cref="FareRule" /> objects to <see cref="FareRuleDto" /> objects and back.
        /// </summary>
        private readonly IConverter<FareRule, FareRuleDto> _fareRuleConverter;

        /// <summary>
        ///     Service used to fetch <see cref="FareRule" /> data from the database.
        /// </summary>
        private readonly FareRuleRepository _fareRuleRepository;

        /// <summary>
        ///     Used for converting <see cref="Route" /> objects to <see cref="RouteDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Route, RouteDto> _routeConverter;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteRepository _routeRepository;

        /// <summary>
        ///     Used for converting <see cref="Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _zoneConverter;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneRepository _zoneRepository;

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
        ///     Creates a <see cref="FareAttribute" /> object in the database.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttributeDto" /> object containing <see cref="FareAttribute" /> data.</param>
        /// <returns>
        ///     <see cref="FareAttributeDto" /> representing the inserted <see cref="FareAttribute" />.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Updates a <see cref="FareAttribute" /> object in the database, using the data stored in the
        ///     <see cref="FareAttributeDto" /> object.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttributeDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttributeDto" /> object containing the updated <see cref="FareAttribute" /> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Deletes a <see cref="FareAttribute" /> from the system.
        /// </summary>
        /// <param name="fareAttribute">
        ///     <see cref="FareAttributeDto" /> representing the <see cref="FareAttribute" /> to be deleted
        ///     from the database.
        /// </param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public void DeleteFareAttribute(FareAttributeDto fareAttribute)
        {
            _fareAttributeRepository.Delete(_fareAttributeConverter.GetEntity(fareAttribute));
        }

        /// <summary>
        ///     Creates a <see cref="FareRule" /> object in the database.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRuleDto" /> object containing <see cref="FareRule" /> data.</param>
        /// <returns>
        ///     <see cref="FareRuleDto" /> representing the inserted <see cref="FareRule" />.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Updates a <see cref="FareRule" /> object in the database, using the data stored in the
        ///     <see cref="FareRuleDto" /> object.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRuleDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRuleDto" /> object containing the updated <see cref="FareRule" /> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Deletes a <see cref="FareRule" /> from the system.
        /// </summary>
        /// <param name="fareRule">
        ///     <see cref="FareRuleDto" /> representing the <see cref="FareRule" /> to be deleted from the
        ///     database.
        /// </param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public void DeleteFareRule(FareRuleDto fareRule)
        {
            _fareRuleRepository.Delete(_fareRuleConverter.GetEntity(fareRule));
        }

        /// <summary>
        ///     Filters <see cref="FareAttribute" /> objects using the supplied <see cref="FareFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttributeDto" /> objects matching the filtering query.
        /// </returns>
        public List<FareAttributeDto> FilterFares(FareFilter filter)
        {
            return _fareAttributeRepository.FilterFares(filter)
                .Select(_fareAttributeConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Filters <see cref="Route" /> objects using the supplied <see cref="RouteFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="RouteDto" /> objects matching the filtering query.
        /// </returns>
        public List<RouteDto> FilterRoutes(RouteFilter filter)
        {
            return _routeRepository.FilterRoutes(filter)
                .Select(_routeConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Filters <see cref="Zone" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter zones by.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto" /> objects matching the filtering query.
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
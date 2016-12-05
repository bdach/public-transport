using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class TripConverter : IConverter<Trip, TripDto>
    {
        private readonly IConverter<Calendar, CalendarDto> _calendarConverter;
        private readonly IConverter<Route, RouteDto> _routeConverter;

        public TripConverter()
        {
            _calendarConverter = new CalendarConverter();
            _routeConverter = new RouteConverter();
        }

        public TripDto GetDto(Trip entity)
        {
            if (entity == null) return null;
            return new TripDto
            {
                Id = entity.Id,
                Route = _routeConverter.GetDto(entity.Route),
                Service = _calendarConverter.GetDto(entity.Service),
                Headsign = entity.Headsign,
                ShortName = entity.ShortName,
                Direction = entity.Direction
            };
        }

        public Trip GetEntity(TripDto dto)
        {
            return new Trip
            {
                Id = dto.Id,
                RouteId = dto.Route?.Id ?? 0,
                ServiceId = dto.Service?.Id ?? 0,
                Headsign = dto.Headsign,
                ShortName = dto.ShortName,
                Direction = dto.Direction
            };
        }
    }
}
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class FareRuleConverter : IConverter<FareRule, FareRuleDto>
    {
        private readonly RouteConverter _routeConverter;
        private readonly ZoneConverter _zoneConverter;

        public FareRuleConverter()
        {
            _routeConverter = new RouteConverter();
            _zoneConverter = new ZoneConverter();
        }

        public FareRuleDto GetDto(FareRule entity)
        {
            if (entity == null) return null;
            return new FareRuleDto
            {
                Id = entity.Id,
                Origin = _zoneConverter.GetDto(entity.Origin),
                Destination = _zoneConverter.GetDto(entity.Destination),
                Route = _routeConverter.GetDto(entity.Route)
            };
        }

        public FareRule GetEntity(FareRuleDto dto)
        {
            return new FareRule
            {
                Id = dto.Id,
                OriginId = dto.Origin?.Id ?? 0,
                DestinationId = dto.Destination?.Id ?? 0,
                RouteId = dto.Route?.Id ?? 0
            };
        }
    }
}

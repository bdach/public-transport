using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class StopConverter : IConverter<Stop, StopDto>
    {
        private readonly IConverter<Street, StreetDto> _streetConverter;
        private readonly IConverter<Zone, ZoneDto> _zoneConverter;

        public StopConverter()
        {
            _streetConverter = new StreetConverter();
            _zoneConverter = new ZoneConverter();
        }

        public StopDto GetDto(Stop entity)
        {
            if (entity == null) return null;
            return new StopDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Street = _streetConverter.GetDto(entity.Street),
                Zone = _zoneConverter.GetDto(entity.Zone),
                ParentStation = GetDto(entity.ParentStation),
                IsStation = entity.IsStation
            };
        }

        public Stop GetEntity(StopDto dto)
        {
            return new Stop
            {
                Id = dto.Id,
                Name = dto.Name,
                StreetId = dto.Street?.Id ?? 0,
                ZoneId = dto.Zone?.Id,
                ParentStationId = dto.ParentStation?.Id,
                IsStation = dto.IsStation
            };
        }
    }
}
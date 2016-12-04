using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class RouteConverter : IConverter<Route, RouteDto>
    {
        private readonly IConverter<Agency, AgencyDto> _agencyConverter;

        public RouteConverter()
        {
            _agencyConverter = new AgencyConverter();
        }

        public RouteDto GetDto(Route entity)
        {
            return new RouteDto
            {
                Id = entity.Id,
                Agency = _agencyConverter.GetDto(entity.Agency),
                ShortName = entity.ShortName,
                LongName = entity.LongName,
                RouteType = entity.RouteType
            };
        }

        public Route GetEntity(RouteDto dto)
        {
            return new Route
            {
                Id = dto.Id,
                AgencyId = dto.Agency?.Id ?? 0,
                ShortName = dto.ShortName,
                LongName = dto.LongName,
                RouteType = dto.RouteType
            };
        }
    }
}
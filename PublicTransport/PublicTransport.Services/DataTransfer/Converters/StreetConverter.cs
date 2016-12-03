using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class StreetConverter : IConverter<Street, StreetDto>
    {
        private readonly IConverter<City, CityDto> _cityConverter;

        public StreetConverter()
        {
            _cityConverter = new CityConverter();
        }

        public StreetDto GetDto(Street entity)
        {
            return new StreetDto
            {
                Id = entity.Id,
                Name = entity.Name,
                City = _cityConverter.GetDto(entity.City)
            };
        }

        public Street GetEntity(StreetDto dto)
        {
            return new Street
            {
                Id = dto.Id,
                Name = dto.Name,
                CityId = dto.City?.Id ?? 0
            };
        }
    }
}
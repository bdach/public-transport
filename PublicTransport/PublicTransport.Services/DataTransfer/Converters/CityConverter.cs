using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class CityConverter : IConverter<City, CityDto>
    {
        public CityDto GetDto(City entity)
        {
            return new CityDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public City GetEntity(CityDto dto)
        {
            return new City
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
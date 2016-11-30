using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class ZoneConverter : IConverter<Zone, ZoneDto>
    {
        public ZoneDto GetDto(Zone entity)
        {
            return new ZoneDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Zone GetEntity(ZoneDto dto)
        {
            return new Zone
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class AgencyConverter : IConverter<Agency, AgencyDto>
    {
        private readonly IConverter<Street, StreetDto> _streetConverter;

        public AgencyConverter()
        {
            _streetConverter = new StreetConverter();
        }

        public AgencyDto GetDto(Agency entity)
        {
            return new AgencyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                Url = entity.Url,
                Regon = entity.Regon,
                Street = _streetConverter.GetDto(entity.Street),
                StreetNumber = entity.StreetNumber
            };
        }

        public Agency GetEntity(AgencyDto dto)
        {
            return new Agency
            {
                Id = dto.Id,
                Name = dto.Name,
                Phone = dto.Phone,
                Url = dto.Url,
                Regon = dto.Regon,
                StreetId = dto.Street?.Id ?? 0,
                StreetNumber = dto.StreetNumber
            };
        }
    }
}
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class FareAttributeConverter : IConverter<FareAttribute, FareAttributeDto>
    {
        private readonly FareRuleConverter _fareRuleConverter;

        public FareAttributeConverter()
        {
            _fareRuleConverter = new FareRuleConverter();
        }

        public FareAttributeDto GetDto(FareAttribute entity)
        {
            return new FareAttributeDto
            {
                Id = entity.Id,
                FareRule = _fareRuleConverter.GetDto(entity.FareRule),
                Price = entity.Price,
                Transfers = entity.Transfers,
                TransferDuration = entity.TransferDuration
            };
        }

        public FareAttribute GetEntity(FareAttributeDto dto)
        {
            return new FareAttribute
            {
                Id = dto.Id,
                FareRuleId = dto.FareRule?.Id ?? 0,
                Price = dto.Price,
                Transfers = dto.Transfers,
                TransferDuration = dto.TransferDuration
            };
        }
    }
}
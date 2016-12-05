using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class StopTimeConverter : IConverter<StopTime, StopTimeDto>
    {
        private readonly IConverter<Stop, StopDto> _stopConverter;
        private readonly IConverter<Trip, TripDto> _tripConverter;

        public StopTimeConverter()
        {
            _stopConverter = new StopConverter();
            _tripConverter = new TripConverter();
        }

        public StopTimeDto GetDto(StopTime entity)
        {
            if (entity == null) return null;
            return new StopTimeDto
            {
                Id = entity.Id,
                Trip = _tripConverter.GetDto(entity.Trip),
                Stop = _stopConverter.GetDto(entity.Stop),
                ArrivalTime = entity.ArrivalTime,
                DepartureTime = entity.DepartureTime,
                StopSequence = entity.StopSequence
            };
        }

        public StopTime GetEntity(StopTimeDto dto)
        {
            return new StopTime
            {
                Id = dto.Id,
                TripId = dto.Trip?.Id ?? 0,
                StopId = dto.Stop?.Id ?? 0,
                ArrivalTime = dto.ArrivalTime,
                DepartureTime = dto.DepartureTime,
                StopSequence = dto.StopSequence
            };
        }
    }
}
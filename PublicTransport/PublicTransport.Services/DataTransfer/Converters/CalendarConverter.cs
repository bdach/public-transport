using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class CalendarConverter : IConverter<Calendar, CalendarDto>
    {
        public CalendarDto GetDto(Calendar entity)
        {
            if (entity == null) return null;
            return new CalendarDto
            {
                Id = entity.Id,
                Monday = entity.Monday,
                Tuesday = entity.Tuesday,
                Wednesday = entity.Wednesday,
                Thursday = entity.Thursday,
                Friday = entity.Friday,
                Saturday = entity.Saturday,
                Sunday = entity.Sunday,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }

        public Calendar GetEntity(CalendarDto dto)
        {
            return new Calendar
            {
                Id = dto.Id,
                Monday = dto.Monday,
                Tuesday = dto.Tuesday,
                Wednesday = dto.Wednesday,
                Thursday = dto.Thursday,
                Friday = dto.Friday,
                Saturday = dto.Saturday,
                Sunday = dto.Sunday,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }
    }
}
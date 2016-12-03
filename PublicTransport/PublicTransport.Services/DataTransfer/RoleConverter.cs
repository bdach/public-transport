using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Converters;

namespace PublicTransport.Services.DataTransfer
{
    public class RoleConverter : IConverter<Role, RoleDto>
    {
        public RoleDto GetDto(Role entity)
        {
            return new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Role GetEntity(RoleDto dto)
        {
            return new Role
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}

using System.Linq;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    public class UserConverter : IConverter<User, UserDto>
    {
        private readonly IConverter<Role, RoleDto> _roleConverter;

        public UserConverter()
        {
            _roleConverter = new RoleConverter();
        }

        public UserDto GetDto(User entity)
        {
            if (entity == null) return null;
            return new UserDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                UserName = entity.UserName,
                Password = entity.Password,
                Roles = entity.Roles.Select(r => _roleConverter.GetDto(r)).ToList()
            };
        }

        public User GetEntity(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                FullName = dto.FullName,
                UserName = dto.UserName,
                Password = dto.Password,
                Roles = dto.Roles.Select(r => _roleConverter.GetEntity(r)).ToList()
            };
        }
    }
}
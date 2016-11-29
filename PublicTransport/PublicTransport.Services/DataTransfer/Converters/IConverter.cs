using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.DataTransfer.Converters
{
    /// <summary>
    ///     Interface for entity-DTO converters.
    /// </summary>
    /// <typeparam name="TEntity">Type of converted entity.</typeparam>
    /// <typeparam name="TDto">Type of associated data transfer object.</typeparam>
    public interface IConverter<TEntity, TDto>
        where TEntity : Entity
    {
        /// <summary>
        ///     Converts an entity object to a DTO.
        /// </summary>
        /// <param name="entity">Entity object to convert.</param>
        /// <returns>Data transfer object to be sent to the user.</returns>
        TDto GetDto(TEntity entity);

        /// <summary>
        ///     Converts a DTO to an entity object. Navigational properties should be omitted and not converted back.
        /// </summary>
        /// <param name="dto">DTO to convert.</param>
        /// <returns>Entity to be used by the EF context.</returns>
        TEntity GetEntity(TDto dto);
    }
}
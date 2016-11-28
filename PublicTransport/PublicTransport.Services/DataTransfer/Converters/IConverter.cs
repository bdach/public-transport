namespace PublicTransport.Services.DataTransfer.Converters
{
    public interface IConverter<TEntity, TDto>
    {
        TDto GetDto(TEntity entity);
        TEntity GetEntity(TDto dto);
    }
}
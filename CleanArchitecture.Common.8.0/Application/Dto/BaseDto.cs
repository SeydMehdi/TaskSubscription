using CleanArchitecture.Common.Core.Common;
using CleanArchitecture.Common.Core.Utils;

namespace CleanArchitecture.Common.Application.Dto;

public interface IBaseDto
{
    string Username { get; set; }
    Guid UserId { get; set; }
    string Roles { get; set; }
}

public class BaseDto : BaseDto<object, BaseModel<long>, long>
{

}
public class BaseDto<TDto> : BaseDto<TDto, BaseModel<long>, long>
{

}

public class BaseDto<TDto, TEntity> : BaseDto<TDto, TEntity, long>
    where TEntity : class, IModel<long>
{

}

public class BaseDto<TDto, TEntity, TKey> : IBaseDto
    where TEntity : class, IModel<TKey>
{
    public TKey Id { get; set; }
    public string Username { get; set; }
    public Guid UserId { get;  set; }

    public string Ip { get; set; }
    public string Roles { get; set; }
    public virtual TEntity ToEntity()
    {
        return this.MapTo<TEntity>();
    }
    public virtual TEntity ToEntity(TEntity entity)
    {
        return this.MapTo(entity);
    }
    public virtual TDto FromEntity(TEntity model)
    {
        return model.MapTo<TDto>();
    }
    public virtual TDto FromEntity(TEntity model, Func<TDto, TDto> modeler)
    {
        return model.MapTo(modeler);
    }
}


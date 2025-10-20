namespace CleanArchitecture.Common.Core.Common
{
    public interface IModel { }

    public interface IModel<TKey> : IModel
    {
        public TKey Id { get; set; }
    }


    public class BaseModel<TKey> : IModel<TKey>
    {
        public TKey Id { get; set; }
    }

    public class BaseModel : BaseModel<long> { }
}

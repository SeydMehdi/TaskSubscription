using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Common.Infra.Common
{
    public interface IEntityConfigured
    {
        void Configure(ModelBuilder modelBuilder);
    }
    public interface IEntityConfigured<T> : IEntityConfigured { }
}



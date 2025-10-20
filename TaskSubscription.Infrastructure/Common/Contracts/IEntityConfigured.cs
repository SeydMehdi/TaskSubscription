using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSubscription.Infrastructure.Common.Contracts
{
    public interface IEntityConfigured
    {
        void Configure(ModelBuilder modelBuilder);
    }
    public interface IEntityConfigured<T> : IEntityConfigured { }
}



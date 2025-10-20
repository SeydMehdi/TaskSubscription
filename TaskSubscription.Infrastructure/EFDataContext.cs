using Microsoft.EntityFrameworkCore;
using Payment.Core.Models.Identities;
using CleanArchitecture.Common.Infra.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace TaskSubscription.Infrastructure
{
    public class EFDataContext : IdentityDbContext<AspnetUser, AspnetRole, Guid>
    {
        public EFDataContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RegisterEntityMaps(typeof(EFDataContext).Assembly);
        }
    }
}

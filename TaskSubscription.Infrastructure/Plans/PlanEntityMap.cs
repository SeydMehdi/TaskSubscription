using Microsoft.EntityFrameworkCore;
using TaskSubscription.Core.Models;
using TaskSubscription.Infrastructure.Common.Contracts;

namespace TaskSubscription.Infrastructure.Plans
{
    public class PlanEntityMap : IEntityConfigured
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Plan>();

            // Primary Key (inherited from BaseModel, assuming it's `Id`)
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            // Required Properties with Constraints
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Description)
                  .HasMaxLength(500);

            entity.Property(p => p.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)") // Adjust precision as needed
                  .HasPrecision(18, 2);

            entity.Property(p => p.Currency)
                  .IsRequired()
                  .HasMaxLength(3) // ISO currency codes like "USD", "EUR"
                  .HasDefaultValue("USD");

            entity.Property(p => p.DurationDays)
                  .IsRequired();

            entity.Property(p => p.Features)
                  .HasMaxLength(4000); // Adjust based on expected content (e.g., JSON or comma-separated)

            entity.Property(p => p.IsActive)
                  .IsRequired()
                  .HasDefaultValue(true);

            entity.Property(p => p.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(p => p.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.IsActive);

            entity.HasMany(p => p.Subscriptions)
                  .WithOne(s => s.Plan)
                  .HasForeignKey(s => s.PlanId)
                  .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}

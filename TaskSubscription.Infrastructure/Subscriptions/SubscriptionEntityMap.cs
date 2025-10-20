using CleanArchitecture.Common.Infra.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSubscription.Core.Models;

namespace TaskSubscription.Infrastructure.Subscriptions
{
    public class SubscriptionEntityMap : IEntityConfigured
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Subscription>();

            // Primary Key
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();

            // Required Properties
            entity.Property(s => s.UserId).IsRequired();
            entity.Property(s => s.PlanId).IsRequired();
            entity.Property(s => s.StartDate).IsRequired();
            entity.Property(s => s.EndDate).IsRequired();
            entity.Property(s => s.Status).IsRequired()
                  .HasDefaultValue(SubscriptionStatus.Active);

            entity.Property(s => s.CancelledAt);
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Indexes (recommended for performance)
            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.PlanId);
            entity.HasIndex(s => s.Status);

            // Relationships
            entity.HasOne(s => s.Plan)
                  .WithMany()
                  .HasForeignKey(s => s.PlanId)
                  .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(s => s.User)
                  .WithMany()
                  .HasForeignKey(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade); 


        }
    }
}

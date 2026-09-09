using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class DealObserverConfiguration : IEntityTypeConfiguration<DealObserver>
    {
        public void Configure(EntityTypeBuilder<DealObserver> builder)
        {
            builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Deal)
                .WithMany(x => x.Observers)
                .HasForeignKey(x => x.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Observer)
                .WithMany()
                .HasForeignKey(x => x.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.DealId,
                x.ObserverId
            }).IsUnique();
        }
    }
}


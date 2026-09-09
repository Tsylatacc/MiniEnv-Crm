using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class DealCustomerConfiguration : IEntityTypeConfiguration<DealCustomer>
    {
        public void Configure(EntityTypeBuilder<DealCustomer> builder)
        {
            builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Deal)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Deals)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.DealId,
                x.CustomerId
            }).IsUnique();
        }
    }
}
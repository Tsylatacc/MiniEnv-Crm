using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class PipelineConfiguration : IEntityTypeConfiguration<Pipeline>
    {
        public void Configure(EntityTypeBuilder<Pipeline> builder)
        {
            builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Stages)
                .WithOne()
                .HasForeignKey(x => x.PipelineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
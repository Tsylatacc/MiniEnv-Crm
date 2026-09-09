using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<RefreshToken>()
                .WithMany()
                .HasForeignKey(x => x.ReplacedByRefreshTokenId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


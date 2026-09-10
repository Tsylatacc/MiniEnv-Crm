using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class SignUpTokenConfiguration : IEntityTypeConfiguration<SignUpToken>
    {
        public void Configure(EntityTypeBuilder<SignUpToken> builder)
        {
            builder.HasIndex(x => new
            { 
                x.Email
            }).IsUnique();
        }
    }
}

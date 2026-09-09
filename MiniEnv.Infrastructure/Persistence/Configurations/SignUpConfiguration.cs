using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class SignUpConfiguration : IEntityTypeConfiguration<SignUp>
    {
        public void Configure(EntityTypeBuilder<SignUp> builder)
        {
            builder.HasIndex(x => new
            {
                x.Email
            }).IsUnique();
        }
    }
}

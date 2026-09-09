using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniEnv.Infrastructure.Persistence.Configurations
{
    public class UserCredentialConfiguration
    {
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.HasIndex(x => new
            {
                x.UserId,
                x.Email
            }).IsUnique();
        }
    }
}

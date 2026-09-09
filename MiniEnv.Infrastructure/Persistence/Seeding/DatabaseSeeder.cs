using MiniEnv.Domain.Entities;
using MiniEnv.Domain.SystemDefaults;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Infrastructure.Persistence.Seeding;

public sealed class DatabaseSeeder
{
    private readonly MiniEnvDbContext _db;

    public DatabaseSeeder(MiniEnvDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await SeedPermissionsAsync(cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken)
    {
        var defaultPermissions = Permissions.Collection;

        var permissions = await _db.Permissions
            .ToListAsync(cancellationToken);

        var existingByName = permissions
            .ToDictionary(x => x.Name);

        foreach (var permission in defaultPermissions)
        {
            var name = permission.Value;

            if (existingByName.ContainsKey(name))
                continue;

            _db.Permissions.Add(
                Permission.Create(name)
            );
        }
    }
}
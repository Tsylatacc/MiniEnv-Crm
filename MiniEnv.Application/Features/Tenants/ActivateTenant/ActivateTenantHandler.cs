using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using MiniEnv.Infrastructure.Persistence;
using JasperFx.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Tenants.ActivateTenant
{
    public class ActivateTenantHandler
    {
        public static async Task<ActivateTenantResult> Handle(
            ActivateTenantCommand command,
            MiniEnvDbContext db,
            ISignUpContext signUpContext,
            IJwtService jwtService,
            TenantId tenantId,
            ILogger<ActivateTenantHandler> logger,
            CancellationToken cancellationToken)
        {
            SignUpToken signUp = await db.SignUpTokens
                .Where(x => x.TokenHash == signUpContext.SignUpTokenHash)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException("Sign-up request not found.");

            if (signUp.UsedAt is not null) throw new ConflictException("Invalid sign-up request.");

            Guid tenantGuid = Guid.Parse(tenantId.Value);
            Tenant tenant = Tenant.Create(tenantGuid, signUp.Email);
            User user = User.Create(signUp.Email, UserStatus.Active, EntityOrigin.System);

            IReadOnlyCollection<Permission> permissions = await db.Permissions.AsNoTracking().ToListAsync(cancellationToken);
            IReadOnlyDictionary<PermissionIndex, Guid> permissionIds = MapPermissionIds(permissions);

            IReadOnlyDictionary<RoleIndex, Domain.Entities.Role> roles = DefaultRoles.CreateForTenant(permissionIds);
            IReadOnlyCollection<Pipeline> pipelines = DefaultPipelines.CreateForTenant();

            user.SetCredential(UserCredential.Create(signUp.Email, user.Id));
            user.SetRole(roles[RoleIndex.Owner].Id);

            await db.Tenants.AddAsync(tenant, cancellationToken);
            await db.Users.AddAsync(user, cancellationToken);
            await db.Roles.AddRangeAsync(roles.Values, cancellationToken);
            await db.Pipelines.AddRangeAsync(pipelines, cancellationToken);

            signUp.SetUsed();
            JwtDto refreshDto = jwtService.GenerateRefreshToken();
            string refreshTokenHash = jwtService.HashToken(refreshDto.Token);

            await db.RefreshTokens.AddAsync(RefreshToken.Create(refreshTokenHash, user.Id, refreshDto.ExpiresAt));

            JwtDto bearerDto = jwtService.GenerateBearerToken(user.Id, tenantGuid, signUp.Email);
            logger.LogInformation("Tenant {TenantId} activated for user {UserId} from sign-up {SignUpId}", tenantGuid, user.Id, signUp.Id);
            return new ActivateTenantResult(
                refreshDto.Token,
                bearerDto.Token,
                refreshDto.ExpiresAt,
                bearerDto.ExpiresAt,
                tenantGuid);
        }

        private static IReadOnlyDictionary<PermissionIndex, Guid> MapPermissionIds(IReadOnlyCollection<Permission> permissions)
        {
            var permissionsByName = permissions.ToDictionary(
                permission => permission.Name);

            return Permissions.Collection.ToDictionary(
                permission => permission.Key,
                permission => permissionsByName[permission.Value].Id);
        }
    }
}

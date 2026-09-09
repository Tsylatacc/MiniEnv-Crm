using MiniEnv.Application.Features.Authentication.ForgotPassword;
using MiniEnv.Application.Features.Authentication.Login;
using MiniEnv.Application.Features.Authentication.Refresh;
using MiniEnv.Application.Features.Authentication.ResetPassword;
using MiniEnv.Application.Features.Authentication.SignUps;
using MiniEnv.Application.Features.Authentication.VerifySignUps;
using MiniEnv.Application.Features.Tenants.ActivateTenant;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class AuthEndpoints
    {
        [AllowAnonymous]
        [WolverinePost("/api/auth/signup")]
        public static async Task SignUp(
            SignUpCommand command,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeAsync(command, cancellationToken);
        }

        [AllowAnonymous]
        [WolverinePost("/api/auth/signup/verify")]
        public static async Task VerifySignUp(
            VerifySignUpCommand command,
            IMessageBus bus,
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            VerifySignUpResponse response = await bus.InvokeAsync<VerifySignUpResponse>(command, cancellationToken);
            if (response.Token != null)
            {
                httpContext.Response.Cookies.Append(
                    "signup_token",
                    response.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        IsEssential = true,
                        Path = "/api/signup",
                        Expires = response.ExpiresAt
                    });
            }
        }

        [AllowAnonymous]
        [WolverinePost("/api/auth/login")]
        public static async Task<LoginResponse> Login(
            LoginCommand command,
            IMessageBus bus,
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            LoginResult result = await bus.InvokeAsync<LoginResult>(command, cancellationToken);
            httpContext.Response.Cookies.Append(
                "refresh_token",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true,
                    Path = "/api/auth/refresh",
                    Expires = result.RefreshTokenExpiresAt
                });

            return new LoginResponse(
                result.BearerToken,
                result.BearerTokenExpiresAt,
                result.TenantId);
        }

        [AllowAnonymous]
        [WolverinePost("/api/auth/password/forgot")]
        public static async Task ForgotPassword(
            ForgotPasswordCommand command,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeAsync(command, cancellationToken);
        }

        [AllowAnonymous]
        [WolverinePost("/api/auth/password/reset")]
        public static async Task ResetPassword(
            ResetPasswordCommand command,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeAsync(command, cancellationToken);
        }

        [Authorize(AuthenticationSchemes = "SignUp")]
        [WolverinePost("/api/auth/signup/activate")]
        public static async Task<ActivateTenantResponse> ActivateTenant(
            IMessageBus bus,
            ISignUpContext signUpContext,
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            ActivateTenantResult result = await bus.InvokeForTenantAsync<ActivateTenantResult>(
                signUpContext.TenantId.ToString(),
                new ActivateTenantCommand(),
                cancellationToken);

            httpContext.Response.Cookies.Append(
                "refresh_token",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true,
                    Path = "/api/auth/refresh",
                    Expires = result.RefreshTokenExpiresAt
                });

            return new ActivateTenantResponse(
                result.BearerToken,
                result.BearerTokenExpiresAt,
                result.TenantId);
        }

        [AllowAnonymous]
        [WolverineGet("/api/auth/refresh")]
        public static async Task<RefreshTokenResponse> Refresh(
            IRefreshContext refreshContext,
            IMessageBus bus,
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            RefreshTokenResult result = await bus.InvokeAsync<RefreshTokenResult>(
                new RefreshTokenCommand(refreshContext.RefreshToken),
                cancellationToken);

            httpContext.Response.Cookies.Append(
                "refresh_token",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true,
                    Path = "/api/auth/refresh",
                    Expires = result.RefreshTokenExpiresAt
                });

            return new RefreshTokenResponse(
                result.BearerToken,
                result.BearerTokenExpiresAt,
                result.TenantId);
        }
    }
}

using JasperFx;
using Microsoft.EntityFrameworkCore;
using MiniEnv.Application.Features.Authentication.VerifySignUps;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Extensions;
using MiniEnv.Infrastructure.Extensions.DependencyInjection;
using MiniEnv.Infrastructure.Persistence;
using MiniEnv.Infrastructure.Persistence.Seeding;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureHostOptions(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(30));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpInfrastructure(builder.Configuration);
builder.Services.AddCorsInfrastructure(builder.Configuration);
builder.Services.AddApiRateLimiting();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseWolverine(options =>
{
    string dbConnection = builder.Configuration.GetConnectionString("db")
        ?? throw new InvalidOperationException("Connection string 'db' not found.");

    options.PersistMessagesWithPostgresql(dbConnection);
    options.UseEntityFrameworkCoreTransactions();
    options.Policies.AutoApplyTransactions();

    options.Discovery.IncludeAssembly(typeof(SignUpCommand).Assembly);
    options.Discovery.IncludeAssembly(typeof(MiniEnvDbContext).Assembly);

    options.UseFluentValidation();
});
builder.Services.AddWolverineHttp();

var app = builder.Build();

await app.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCorsInfrastructure();

app.UseApiRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapWolverineEndpoints(options =>
{
    options.ServiceProviderSource = ServiceProviderSource.FromHttpContextRequestServices;
    options.SourceServiceFromHttpContext<ICurrentUser>();
});

return await app.RunJasperFxCommands(args);

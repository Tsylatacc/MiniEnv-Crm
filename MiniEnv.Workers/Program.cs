using MiniEnv.Infrastructure.Extensions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);

builder.UseWolverine(options =>
{
    string dbConnection = builder.Configuration.GetConnectionString("MiniEnvdb")
        ?? throw new InvalidOperationException("Connection string 'MiniEnvdb' not found.");

    string rabbitConnection = builder.Configuration.GetConnectionString("rabbitmq")
        ?? throw new InvalidOperationException("Connection string 'rabbitmq' not found.");

    options.PersistMessagesWithPostgresql(dbConnection);
    options.UseEntityFrameworkCoreTransactions();
    options.Policies.AutoApplyTransactions();

    options.UseRabbitMq(rabbitConnection)
        .AutoProvision()
        .BindExchange("signup-requested")
        .ToQueue("signup-requested-queue")
        .BindExchange("password-reset-requested")
        .ToQueue("password-reset-requested-queue")
        .BindExchange("user-invitation-requested")
        .ToQueue("user-invitation-requested-queue");

    options.ListenToRabbitQueue("signup-requested-queue")
        .UseDurableInbox();
    options.ListenToRabbitQueue("password-reset-requested-queue")
        .UseDurableInbox();
    options.ListenToRabbitQueue("user-invitation-requested-queue")
        .UseDurableInbox();

    options.Discovery.IncludeAssembly(typeof(MiniEnvDbContext).Assembly);

    options.OnException<Exception>()
        .RetryWithCooldown(TimeSpan.FromMilliseconds(150))
        .Then.MoveToErrorQueue();
});

var host = builder.Build();
host.Run();
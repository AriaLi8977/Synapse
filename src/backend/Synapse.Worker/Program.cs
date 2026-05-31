using Synapse.Worker;
using Azure.Messaging.ServiceBus;
using DotNetEnv;
using Synapse.Worker.Services;
using Synapse.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Synapse.Infrastructure.Data;
using Synapse.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Synapse.Infrastructure;
Env.Load("../../../.env");

var builder = Host.CreateApplicationBuilder(args);

// Add DbContext
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddHttpClient<IAiService, AiService>();

builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["ServiceBus:ConnectionString"];
    return new ServiceBusClient(connectionString);
});

builder.Services.AddHostedService<NoteProcessWorker>();

var host = builder.Build();
host.Run();

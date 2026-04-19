using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Synapse.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var clent = new ServiceBusClient(_configuration["ServiceBus"]);
        var processor = clent.CreateProcessor("NotesQueue", new ServiceBusProcessorOptions());
        processor.ProcessMessageAsync += HandleMessage;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task HandleMessage(ProcessMessageEventArgs args)
    {
        var json = args.Message.Body.ToString();
        var message = System.Text.Json.JsonSerializer.Deserialize<NoteMessage>(json);
        _logger.LogInformation($"Processing note: Id:{message.NoteId},Content:{message.Content}");
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Message handler encountered an exception");
        return Task.CompletedTask;
    }
}

public class NoteMessage
{
    public Guid NoteId { get; set; }
    public string Content { get; set; }
}

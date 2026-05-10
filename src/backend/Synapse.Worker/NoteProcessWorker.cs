using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Synapse.Worker.Services;
using Synapse.Domain.Enums;

namespace Synapse.Worker;

public class NoteProcessWorker : BackgroundService
{
    private readonly ILogger<NoteProcessWorker> _logger;
    private readonly ServiceBusProcessor _processor;
    private readonly IAiService _ai;
    private readonly INoteRepository _noteRepository;
    private readonly INotificationService _notifier;
    public NoteProcessWorker(ILogger<NoteProcessWorker> logger,
                            ServiceBusClient client,
                            IAiService ai,
                            INoteRepository noteRepository,
                            INotificationService notifier)
    {
        _logger = logger;
        _processor = client.CreateProcessor("NotesQueue");
        _ai = ai;
        _noteRepository = noteRepository;
        _notifier = notifier;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += HandleMessage;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task HandleMessage(ProcessMessageEventArgs args)
    {
        var json = args.Message.Body.ToString();
        var message = System.Text.Json.JsonSerializer.Deserialize<NoteMessage>(json);
        if (message == null) return;
        _logger.LogInformation($"Processing note: Id:{message.NoteId}");

        var noteId = message.NoteId;
        var userId = message.Content;

        var note = await _noteRepository.GetByIdAsync(noteId);
        if (note == null) return;

        note.Status = NoteStatus.Processing;
        await _noteRepository.UpdateAsync(note);
        await _notifier.NotifyNoteProcessing(note.Id);

        try
        {
            var summary = await _ai.SummarizeAsync(message.Content ?? "");
            note.Summary = summary;
            note.Status = NoteStatus.Completed;
        }
        catch
        {
            note.Status = NoteStatus.Failed;
        }

        await _noteRepository.UpdateAsync(note);

        await _notifier.NotifyNoteCompleted(note.Id, note.Summary??"No summary generated", note.UserId);
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
    public string? Content { get; set; }
}

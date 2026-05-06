public interface INotificationService
{
    Task NotifyNoteCompleted(Guid noteId, string summary, Guid userId);

    Task NotifyNoteProcessing(Guid noteId);
}
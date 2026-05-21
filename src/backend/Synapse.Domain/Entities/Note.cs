using Synapse.Domain.Enums;

namespace Synapse.Domain.Entities;

public class Note
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public NoteStatus Status { get; set; }
}
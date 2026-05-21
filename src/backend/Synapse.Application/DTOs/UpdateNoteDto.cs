using System.ComponentModel.DataAnnotations;

namespace Synapse.Application.DTOs
{
    public class UpdateNoteDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
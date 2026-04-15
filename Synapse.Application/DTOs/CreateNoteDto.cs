using System.ComponentModel.DataAnnotations;

namespace Synapse.Application.DTOs
{
    public class CreateNoteDto
    {
        [Required]
        [MaxLength(500)]    
        public string Content { get; set; } = string.Empty;
    }
}
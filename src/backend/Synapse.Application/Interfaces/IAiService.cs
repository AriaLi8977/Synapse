
public interface IAiService
{
    Task<NoteAiResultDto> SummarizeAsync(string content);
}
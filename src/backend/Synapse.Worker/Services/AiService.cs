using System.Text;
using System.Text.Json;
using Synapse.Application.Interfaces;
using Synapse.Application.DTOs;


namespace Synapse.Worker.Services;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public AiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;

        _apiKey = config["DeepSeek:ApiKey"]
            ?? throw new Exception("DeepSeek ApiKey missing");

        _model = config["DeepSeek:Model"]
            ?? throw new Exception("DeepSeek Model missing");
    }

    public async Task<NoteAiResultDto> SummarizeAsync(string content)
    {
        var systemPrompt = """
            You are a note analysis assistant.

            Generate:
            1. A concise professional title
            2. A short bullet-point summary

            Return ONLY valid JSON in this format:

            {
              "title": "...",
              "summary": "..."
            }
            """;

        var userPrompt = content;

        var aiResponse = await SendChatAsync(systemPrompt, userPrompt);

        try
        {
            var result = JsonSerializer.Deserialize<NoteAiResultDto>(
                aiResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
            {
                throw new Exception("AI returned null result.");
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to parse AI response: {ex.Message}");
            Console.WriteLine($"Raw AI response: {aiResponse}");

            throw new Exception(
                "Failed to parse AI response into NoteAiResult.");
        }
    }



    private async Task<string> SendChatAsync(string systemPrompt, string userPrompt)
    {

        Console.WriteLine("Sending request to DeepSeek...");
        var requestBody = new
        {
            model = _model,
            messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.deepseek.com/chat/completions");

        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseContent);

        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(responseContent);

        var result = doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return result ?? "";
    }
}

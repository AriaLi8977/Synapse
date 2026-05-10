using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Synapse.Application.Interfaces;

namespace Synapse.Worker.Services;
public class AiService : IAiService
{
    private readonly ChatClient _chatClient;

    public AiService(IConfiguration config)
    {
        var endpoint = config["AzureOpenAI:Endpoint"];
        var apiKey = config["AzureOpenAI:ApiKey"];
        var deploymentName = config["AzureOpenAI:DeploymentName"];
        if (string.IsNullOrEmpty(endpoint))
            throw new Exception("AzureOpenAI Endpoint missing");

        if (string.IsNullOrEmpty(apiKey))
            throw new Exception("AzureOpenAI ApiKey missing");

        if (string.IsNullOrEmpty(deploymentName))
            throw new Exception("AzureOpenAI DeploymentName missing");

        var client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        _chatClient = client.GetChatClient(deploymentName);
    }

    public async Task<string> SummarizeAsync(string content)
    {
        var response = await _chatClient.CompleteChatAsync(
            $"Summarize the following content:\n\n{content}");

        return response.Value.Content[0].Text;
    }
}
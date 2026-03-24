namespace AiGarden.AiService.Infrastructure.Options;

public sealed class NvidiaOptions
{
    public const string SectionName = "Nvidia";

    public string ApiKey { get; init; } = string.Empty;
    public string ChatCompletionsUrl { get; init; } = "https://integrate.api.nvidia.com/v1/chat/completions";
    public string DefaultModel { get; init; } = "mistralai/mistral-large-3-675b-instruct-2512";
}

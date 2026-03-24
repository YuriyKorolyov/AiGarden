namespace AiGarden.AiService.Infrastructure.Options;

public sealed class OllamaOptions
{
    public const string SectionName = "Ollama";

    public string BaseUrl { get; init; } = "http://localhost:11434";
    public string DefaultModel { get; init; } = "qwen3.5:9b";
}

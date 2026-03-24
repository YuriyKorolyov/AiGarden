using System.Net.Http.Json;
using System.Text.Json;
using AiGarden.AiService.Core.Services;
using AiGarden.AiService.Infrastructure.Options;
using AiGarden.Contracts.Enums;
using Microsoft.Extensions.Options;

namespace AiGarden.AiService.Infrastructure.Providers;

public sealed class OllamaPlantDiagnosisProvider(
    IHttpClientFactory httpClientFactory,
    IOptions<OllamaOptions> options) : IPlantDiagnosisProvider
{
    private readonly OllamaOptions _options = options.Value;
    public AiProviderType ProviderType => AiProviderType.Ollama;

    public async Task<PlantDiagnosisResult> DiagnoseAsync(string photoUrl, string? userPrompt, string model, CancellationToken cancellationToken)
    {
        var imageClient = httpClientFactory.CreateClient("photos");
        var imageBytes = await imageClient.GetByteArrayAsync(photoUrl, cancellationToken);
        var imageBase64 = Convert.ToBase64String(imageBytes);

        var payload = new
        {
            model,
            stream = false,
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "Ты бот-агроном. Отвечай по-русски: диагноз растения, вероятность, возможная причина и 3 действия для восстановления."
                },
                new
                {
                    role = "user",
                    content = userPrompt ?? "Проанализируй состояние растения по фото.",
                    images = new[] { imageBase64 }
                }
            }
        };

        var client = httpClientFactory.CreateClient("ollama");
        using var response = await client.PostAsJsonAsync("/api/chat", payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        var root = document.RootElement;
        var diagnosis = root.GetProperty("message").GetProperty("content").GetString()
            ?? "Ollama did not return diagnosis.";
        var promptTokens = root.TryGetProperty("prompt_eval_count", out var promptElement) ? promptElement.GetInt32() : 0;
        var completionTokens = root.TryGetProperty("eval_count", out var completionElement) ? completionElement.GetInt32() : 0;

        return new PlantDiagnosisResult(diagnosis.Trim(), model, promptTokens, completionTokens);
    }
}

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AiGarden.AiService.Core.Services;
using AiGarden.AiService.Infrastructure.Options;
using AiGarden.Contracts.Enums;
using Microsoft.Extensions.Options;

namespace AiGarden.AiService.Infrastructure.Providers;

public sealed class NvidiaPlantDiagnosisProvider(
    IHttpClientFactory httpClientFactory,
    IOptions<NvidiaOptions> options) : IPlantDiagnosisProvider
{
    private readonly NvidiaOptions _options = options.Value;
    public AiProviderType ProviderType => AiProviderType.Nvidia;

    public async Task<PlantDiagnosisResult> DiagnoseAsync(string photoUrl, string? userPrompt, string model, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("nvidia");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        var payload = new
        {
            model,
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "You are a plant health expert. Return the answer in Russian with diagnosis, risk level, possible cause and 3 care recommendations."
                },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = userPrompt ?? "Проанализируй состояние растения по фото." },
                        new { type = "image_url", image_url = new { url = photoUrl } }
                    }
                }
            },
            temperature = 0.2,
            top_p = 0.7,
            max_tokens = 600
        };

        using var response = await client.PostAsJsonAsync(_options.ChatCompletionsUrl, payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        var root = document.RootElement;
        var diagnosis = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()
            ?? "AI did not return diagnosis.";

        var usage = root.TryGetProperty("usage", out var usageElement) ? usageElement : default;
        var promptTokens = usage.ValueKind == JsonValueKind.Object && usage.TryGetProperty("prompt_tokens", out var promptElement)
            ? promptElement.GetInt32()
            : 0;
        var completionTokens = usage.ValueKind == JsonValueKind.Object && usage.TryGetProperty("completion_tokens", out var completionElement)
            ? completionElement.GetInt32()
            : 0;

        return new PlantDiagnosisResult(diagnosis.Trim(), model, promptTokens, completionTokens);
    }
}

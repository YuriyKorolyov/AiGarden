using System.Text.Json;
using AiGarden.AiService.UseCases.Abstractions;
using AiGarden.AiService.UseCases.Commands;
using AiGarden.AiService.UseCases.Queries;
using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Requests;
using AiGarden.Contracts.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiGarden.AiService.Controllers;

[ApiController]
[Route("api/analyses")]
[Authorize]
public sealed class AnalysesController(
    ICurrentUserAccessor currentUserAccessor,
    IValidator<StartPlantAnalysisCommand> validator,
    ICommandHandler<StartPlantAnalysisCommand, AnalysisAcceptedResponse> startHandler,
    IQueryHandler<GetPlantAnalysisQuery, AnalysisResultResponse?> getHandler,
    IAnalysisEventBroker analysisEventBroker) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Start([FromBody] CreateAnalysisRequest request, CancellationToken cancellationToken)
    {
        var command = new StartPlantAnalysisCommand(
            currentUserAccessor.GetRequiredUserId(),
            request.PhotoUrl,
            request.UserPrompt,
            request.Provider,
            request.Model);

        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var response = await startHandler.HandleAsync(command, cancellationToken);
        return Accepted(response.EventsUrl, response);
    }

    [HttpGet("{analysisId:guid}")]
    public async Task<IActionResult> Get(Guid analysisId, CancellationToken cancellationToken)
    {
        var result = await getHandler.HandleAsync(new GetPlantAnalysisQuery(analysisId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{analysisId:guid}/events")]
    public async Task Stream(Guid analysisId, CancellationToken cancellationToken)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");

        await foreach (var analysisEvent in analysisEventBroker.SubscribeAsync(analysisId, cancellationToken))
        {
            var json = JsonSerializer.Serialize(analysisEvent);
            await Response.WriteAsync($"event: analysis{Environment.NewLine}", cancellationToken);
            await Response.WriteAsync($"data: {json}{Environment.NewLine}{Environment.NewLine}", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}

using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.HistoryService.UseCases.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiGarden.HistoryService.Controllers;

[ApiController]
[Route("api/history")]
[Authorize]
public sealed class HistoryController(
    ICurrentUserAccessor currentUserAccessor,
    IQueryHandler<GetUserHistoryQuery, IReadOnlyCollection<AnalysisHistoryItemResponse>> handler) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IReadOnlyCollection<AnalysisHistoryItemResponse>> GetMyHistory(CancellationToken cancellationToken) =>
        await handler.HandleAsync(new GetUserHistoryQuery(currentUserAccessor.GetRequiredUserId()), cancellationToken);
}

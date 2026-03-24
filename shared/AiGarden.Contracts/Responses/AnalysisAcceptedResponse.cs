using AiGarden.Contracts.Enums;

namespace AiGarden.Contracts.Responses;

public sealed record AnalysisAcceptedResponse(Guid AnalysisId, AnalysisStatus Status, string EventsUrl);

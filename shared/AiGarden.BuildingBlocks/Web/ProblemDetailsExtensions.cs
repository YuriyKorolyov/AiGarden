using AiGarden.BuildingBlocks.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiGarden.BuildingBlocks.Web;

public static class ProblemDetailsExtensions
{
    public static IResult ToProblem(this Result result) =>
        Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: result.Error);

    public static IResult ToProblem<T>(this Result<T> result) =>
        Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: result.Error);
}

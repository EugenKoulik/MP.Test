using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace MP.API.Extensions;

internal static class ControllerExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsFailed)
        {
            return new ObjectResult(result.ToString())
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        return new OkObjectResult(result.Value);
    }
    
    public static ActionResult ToActionResultWithoutValue(this Result result)
    {
        if (result.IsFailed)
        {
            return new ObjectResult(result.ToString())
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        return new OkResult();
    }
}
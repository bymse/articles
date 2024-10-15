using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Bymse.Articles.Apis.Public;

public class ValidationExceptionFilter : ExceptionFilterAttribute
{
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            var factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(e => e.Key, e => e.Select(r => r.ErrorCode).ToArray());

            var problemDetails = factory.CreateProblemDetails(context.HttpContext, 400, "Validation error");
            problemDetails.Extensions["errors"] = errors;

            context.Result = new BadRequestObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
            context.ExceptionHandled = true;
        }

        return Task.CompletedTask;
    }
}
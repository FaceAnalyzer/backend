using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FaceAnalyzer.Api.Service.Middlewares;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ErrorHandlerMiddleware(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "Unhandled Exception";
            switch (ex)
            {
                case ProjectGrantPermissionException 
                    or ProjectRevokePermissionException 
                    or InvalidCredentialException 
                    or InvalidArgumentsException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    title = ex.GetType().Name;
                    break;
                case EntityNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    title = ex.GetType().Name;
                    break;
            }

            var problem = _problemDetailsFactory.CreateProblemDetails(
                httpContext: context,
                statusCode: statusCode,
                title: title,
                detail: ex.Message
            );
            Console.WriteLine(ex);


            await SetHttpContextResponse(context, problem);
        }
    }

    private async Task SetHttpContextResponse(HttpContext context, ProblemDetails problem)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        if (problem.Status.HasValue)
        {
            statusCode = problem.Status.Value;
        }

        context.Response.Headers.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        var response = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(response);
    }
}
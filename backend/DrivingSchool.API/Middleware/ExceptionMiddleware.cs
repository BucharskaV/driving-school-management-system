using System.Net;
using System.Text.Json;
using DrivingSchool.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Middleware;

public class ExceptionMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly IReadOnlyDictionary<Type, HttpStatusCode> StatusCodeMap = new Dictionary<Type, HttpStatusCode>
    {
        [typeof(DuplicateEnrollmentException)] = HttpStatusCode.BadRequest,
        
        [typeof(ArgumentNullException)] = HttpStatusCode.NotFound,
        [typeof(CategoryNotFoundException)] = HttpStatusCode.NotFound,
        [typeof(StudentNotFoundException)] = HttpStatusCode.NotFound,
        [typeof(CourseNotFoundException)] = HttpStatusCode.NotFound,
    };
 
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
 
    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
 
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
 
        var problem = BuildProblemDetails(context, exception, statusCode);
 
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
 
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, SerializerOptions));
    }
 
    private ProblemDetails BuildProblemDetails(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        var isKnownException = StatusCodeMap.ContainsKey(exception.GetType());
 
        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Title = statusCode.ToString(),
            Status = (int)statusCode,
            Detail = isKnownException || _environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred. Please try again later.",
            Instance = context.Request.Path
        };
        problem.Extensions["traceId"] = context.TraceIdentifier;
        if (_environment.IsDevelopment() && !isKnownException)
            problem.Extensions["stackTrace"] = exception.StackTrace;
 
        return problem;
    }
 
    private static HttpStatusCode GetStatusCode(Exception exception) =>
        StatusCodeMap.TryGetValue(exception.GetType(), out var statusCode) ? statusCode : HttpStatusCode.InternalServerError;
}
 
public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}
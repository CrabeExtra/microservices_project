using Identity.Application.Exceptions;
using Identity.Database.Exceptions;
using Identity.Domain.Exceptions;
using Identity.Messaging.Exceptions;
using SQLitePCL;

public class ErrorResponse
{
    public string Error { get; set; } = "";
}

public class ExceptionMiddleware
{


    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private static Task WriteError(HttpContext context, int statusCode, string message)
{
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = statusCode;

    return context.Response.WriteAsJsonAsync(new ErrorResponse
    {
        Error = message
    });
}

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await WriteError(context, 400, ex.Message);
        }
        catch (ServiceException ex)
        {
            await WriteError(context, 400, ex.Message);
        }
        catch (RepositoryException ex)
        {
            await WriteError(context, 409, ex.Message);
        }
        catch (MessageException ex)
        {
            await WriteError(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await WriteError(context, 500, "An unexpected error occurred");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using MongoDB.Driver;

public class ValidateCaptchaMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateCaptchaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        string userInput = context.Request.Headers["X-CaptchaCode"].FirstOrDefault() ?? string.Empty;
        var storedCode = context.Session.GetString("CaptchaCode");

        if (string.IsNullOrEmpty(userInput))
        {
            context.Response.StatusCode = 400; 
            await context.Response.WriteAsync("Captcha missing. Please provide the X-CaptchaCode header.");
            return;
        }

        if (string.IsNullOrEmpty(storedCode) || !userInput.Equals(storedCode, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Captcha invalid.");
            return;
        }
        Console.WriteLine("Validated");
        await _next(context);
=======
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateCaptcha : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {   
        var httpContext = context.HttpContext;
        var session = httpContext.Session;

        string code = httpContext.Request.Headers["X-CaptchaCode"].FirstOrDefault() ?? string.Empty;

        var storedCode = session.GetString("CaptchaCode");

        Console.WriteLine("Stored code: " + storedCode);

        if(string.IsNullOrEmpty(code) || !code.Equals(storedCode, StringComparison.OrdinalIgnoreCase)) {

            context.Result = new BadRequestObjectResult(new { success = false, message = "Invalid captcha." });
            return;
        }
        

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing
>>>>>>> 10236a3 (feat: session)
    }
}

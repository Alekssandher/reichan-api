using Microsoft.AspNetCore.Mvc;
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
    }
}

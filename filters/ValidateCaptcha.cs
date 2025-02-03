using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateCaptcha : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var session = httpContext.Session;

        

        string code = httpContext.Request.Headers["X-CaptchaCode"].FirstOrDefault() ?? string.Empty;

        var storedCode = session.GetString("CaptchaCode");

        if (storedCode == null)
        {
            Console.WriteLine("No captcha code found in session.");
            context.Result = new BadRequestObjectResult(new { success = false, message = "Captcha code not found." });
            return;
        }

        Console.WriteLine("Stored code: " + storedCode);

        if (string.IsNullOrEmpty(code) || !code.Equals(storedCode, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new BadRequestObjectResult(new { success = false, message = "Invalid captcha." });
            return;
        }

        // httpContext.Session.Remove("CaptchaCode");
        // httpContext.Session.Remove("CaptchaCodeExpiration");
        // httpContext.Session.Set("InColdown", BitConverter.GetBytes(DateTime.UtcNow.AddSeconds(30).ToBinary()));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var httpContext = context.HttpContext;
        var session = httpContext.Session;

        session.Remove("CaptchaCode");
    }
}
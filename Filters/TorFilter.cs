public class SecureApiMiddleware
{
    private readonly RequestDelegate _next;

    public SecureApiMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (string.IsNullOrEmpty(forwardedFor) || !forwardedFor.Contains(".onion"))
        {
            context.Response.StatusCode = 403; 
            await context.Response.WriteAsync("Access denied. Only Tor traffic is allowed.");
            return;
        }

        await _next(context);
    }
}

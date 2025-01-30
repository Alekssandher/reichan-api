public class SecureApiMiddleware
{
    private readonly RequestDelegate _next;

    public SecureApiMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var isTorConnection = context.Request.Headers["X-Forwarded-For"].Any(ip => ip.Contains(".onion"));

        if (!isTorConnection)
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Access denied. Only Tor traffic is allowed.");
            return;
        }

        await _next(context);
    }
}

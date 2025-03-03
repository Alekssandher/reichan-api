using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using reichan_api.src.DTOs.Responses;

namespace reichan_api.Filters.captcha
{
    public class ValidateCaptcha : IActionFilter
    {
        private static readonly BadRequestObjectResult badRequest = new ( new BadRequest("Bad Request", "Captcha code not found or invalid."));
      
        public void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;
            ISession session = httpContext.Session;

            

            string code = httpContext.Request.Headers["X-CaptchaCode"].FirstOrDefault() ?? string.Empty;

            string? storedCode = session.GetString("CaptchaCode");

            if (storedCode == null)
            {
                context.Result = badRequest;
                return;
            }

            if (string.IsNullOrEmpty(code) || !code.Equals(storedCode, StringComparison.Ordinal))
            {
                context.Result = badRequest;
                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            HttpContext httpContext = context.HttpContext;
            ISession session = httpContext.Session;

            session.Remove("CaptchaCode");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateCategory : IActionFilter
{
    private readonly string[] categories = Environment.GetEnvironmentVariable("CATEGORIES")!.Split(',');

    public void OnActionExecuting(ActionExecutingContext context)
    {

        string category = context.HttpContext.Request.Query["category"].ToString().ToLower();

        if (string.IsNullOrEmpty(category))
        {
            context.Result = new BadRequestObjectResult(new { success = false, message = "Category is required." });
            return;
        }

        if (!categories.Contains(category))
        {
            context.Result = new BadRequestObjectResult(new { success = false, message = "Category does not exist." });
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing
    }
}

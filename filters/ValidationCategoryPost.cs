using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateCategoryPost : IActionFilter
{
    private readonly string[] categories = Environment.GetEnvironmentVariable("CATEGORIES")!.Split(',');

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var postDto = context.ActionArguments.Values
            .FirstOrDefault(arg => arg is CreatePostDto || arg is CreateSignedPostDto);

        if (postDto == null)
        {
            context.Result = new BadRequestObjectResult(new { success = false, message = "Invalid data." });
            return;
        }

        string category = null;

        if (postDto is CreatePostDto post)
        {
            category = post.Category;
        }
        else if (postDto is CreateSignedPostDto signedPost)
        {
            category = signedPost.Category;
        }

        if (string.IsNullOrEmpty(category))
        {
            context.Result = new BadRequestObjectResult(new { success = false, message = "Invalid category." });
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

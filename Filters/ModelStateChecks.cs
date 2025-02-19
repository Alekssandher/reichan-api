using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace reichan_api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModelStateCheck :Attribute, IActionFilter
    {
        private List<string> ListModelErros(ActionContext context) =>
                        [.. context.ModelState
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)];

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.ModelState.IsValid)
            {
                List<string> errors = ListModelErros(context);

                var result = new
                {
                    status = 400,
                    errors
                };

                context.Result = new BadRequestObjectResult(result);
                return;
            }
            return;
        }
    }
}
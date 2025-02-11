
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using reichan_api.src.DTOs.Global;

namespace reichan_api.Filters {
    public class ValidateIdAttribute : IActionFilter
    {
        private static readonly Regex _validIdRegex = new(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (context.ActionArguments.TryGetValue("id", out object? value))
            {
                string? id = value as string;

                BadRequest? error = id switch
                {
                    _ when string.IsNullOrEmpty(id) => new BadRequest("Empty ID", "You must provide an ID."),
                    _ when !_validIdRegex.IsMatch(id) => new BadRequest("Invalid ID", "The provided ID contains invalid characters."),
                    _ when !ObjectId.TryParse(id, out _) => new BadRequest("Invalid ID", "The provided ID is not valid."),
                    _ => null
                };

                if (error != null)
                {
                    context.Result = new BadRequestObjectResult(error);
                }

               
            }

            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}


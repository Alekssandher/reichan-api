
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using reichan_api.src.DTOs.Responses;

namespace reichan_api.Filters {
    public class ValidateIdAttribute : IActionFilter
    {
        private static readonly BadRequestObjectResult EmptyIdError = new( new BadRequest("Empty ID", "You must provide an ID.") );
        private static readonly BadRequestObjectResult InvalidCharError =  new( new BadRequest("Invalid ID", "The provided ID contains invalid characters.") );
        private static readonly BadRequestObjectResult IdTooLong =  new( new ContentTooLarge("Too Long ID", "The provided ID is too long.") );        
        private static readonly Regex _validIdRegex = new(@"^\d+$", RegexOptions.Compiled);

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (context.ActionArguments.TryGetValue("id", out object? value))
            {
                string? id = value as string;

                if (string.IsNullOrEmpty(id))
                {
                    context.Result = EmptyIdError;
                    return;
                }

                if (!_validIdRegex.IsMatch(id))
                {

                    context.Result = InvalidCharError;
                    return;
                }

                if(id.Length > 20) {
                    context.Result = IdTooLong;
                    return;
                }
            }

            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }
}


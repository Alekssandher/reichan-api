using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using reichan_api.src.DTOs.Global;

namespace reichan_api.Filters {
    public class ValidateQueryAttribute : IActionFilter 
    {
        private const int MaxQueryLength = 10;
        private const int MaxLimit = 20;
        private const int MinLimit = 1;
        private const int MinSkip = 0;
        
        private static readonly BadRequestObjectResult QueryTooLong = new(
            new ContentTooLarge("Too Long Query", "The provided Query is too long.")
        );        
        private static readonly BadRequestObjectResult InvalidQuery = new(
            new BadRequest("Invalid Query", "The provided Query contains invalid characters.")
        );
        private static readonly BadRequestObjectResult InvalidNumber = new(
            new BadRequest("Invalid Number", "The provided number is invalid or out of range.")
        );

        private static readonly Regex _validLettersRegex = new(@"^[a-zA-Z]+$", RegexOptions.Compiled);

        public void OnActionExecuting(ActionExecutingContext context) {
            var queryParams = context.HttpContext.Request.Query;

            string[] stringKeys = ["category", "author"];
            foreach (var key in stringKeys)
            {

                if (!queryParams.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
                    continue;

                string queryValue = value.ToString();

                if (queryValue.Length > MaxQueryLength)
                {
                    context.Result = QueryTooLong;
                    return;
                }

                if (!_validLettersRegex.IsMatch(queryValue) )
                {
                    context.Result = InvalidQuery;
                    return;
                }
            }

            if (queryParams.TryGetValue("limit", out var limitValue) && !string.IsNullOrWhiteSpace(limitValue))
            {
                if (!int.TryParse(limitValue, out int limit) || limit < MinLimit || limit > MaxLimit)
                {
                    context.Result = InvalidNumber;
                    return;
                }
            }

            if (queryParams.TryGetValue("skip", out var skipValue) && !string.IsNullOrWhiteSpace(skipValue))
            {
                if (!int.TryParse(skipValue, out int skip) || skip < MinSkip)
                {
                    context.Result = InvalidNumber;
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

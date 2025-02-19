using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using reichan_api.src.DTOs.Responses;

namespace reichan_api.Filters {
    public class ValidateGetMedia(ILogger<ValidateGetMedia> logger) : IActionFilter 
    {
        private const int MaxCategoryLength = 10;
        private const int MaxFileNameLength = 25;
        private readonly ILogger<ValidateGetMedia> _logger = logger;
        private static readonly Regex _validCategoryRegex = new(@"^[a-zA-Z]+$", RegexOptions.Compiled);
        private static readonly Regex _validFileNameRegex = new(@"^[a-zA-Z0-9.]+$", RegexOptions.Compiled);

        public void OnActionExecuting(ActionExecutingContext context) {
            var routeValues = context.RouteData.Values;

            if (!routeValues.TryGetValue("category", out var categoryObj) ||
                !routeValues.TryGetValue("fileName", out var fileNameObj) ||
                categoryObj is not string category ||
                fileNameObj is not string fileName)
            {
                _logger.LogWarning("Os parâmetros 'category' e 'fileName' não foram encontrados na rota.");
                return;
            }

            
            if (category.Length > MaxCategoryLength || fileName.Length > MaxFileNameLength)
            {
                context.Result = new ObjectResult(new ContentTooLarge(
                    "Request Too Long", "You are passing too long arguments"))
                {
                    StatusCode = StatusCodes.Status414RequestUriTooLong
                };
                return;
            }

            if (!_validCategoryRegex.IsMatch(category) || !_validFileNameRegex.IsMatch(fileName))
            {
                Console.WriteLine(category);
                context.Result = new BadRequestObjectResult(new BadRequest ("Invalid format", "Category or FileName contains invalid characters"));
                
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

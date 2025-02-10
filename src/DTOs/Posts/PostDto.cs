using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace reichan_api.src.DTOs.Posts {
    
    [ModelBinder(BinderType = typeof(StrictJsonModelBinder))]
    public record class PostDto {
        
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
        public string? Title { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "Content chars must be between 1 - 600 chars")]
        [DataType(DataType.Text)]
        public string? Content { get; init; }

        [Required(ErrorMessage = "Image is required.")]
        [RegularExpression(@"^[\w,\s-]+\.(jpg|jpeg|png|gif|webpm|mp4|ogg)$", ErrorMessage = "Invalid image or video format are allowed.")]
        public string? Image { get; init; }

        [Required(ErrorMessage = "Category is required.")]
        [EnumDataType(typeof(PostCategory), ErrorMessage = "Invalid category.")]
        public string? Category { get; init; }

        public string Author { get; init; } = "Anonymous";
        public string? AuthorPubKey { get; init; } = null;
        public string? Signature { get; init; } = null;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public bool Active { get; init; } = true;
        public int UpVotes { get; init; } = 0;
        public int DownVotes { get; init; } = 0;
    }

    public enum PostCategory {
        News,
        Blog,
        Tutorial,
        Review
    }

    public class StrictJsonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var request = bindingContext.HttpContext.Request;

        try
        {
            using (var reader = new StreamReader(request.Body))
            {
                var body = reader.ReadToEndAsync().Result;
                var model = JsonSerializer.Deserialize(body, bindingContext.ModelType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (model == null)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid JSON format.");
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}

}

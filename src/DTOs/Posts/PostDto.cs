using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Posts
{
    public record class ThreadDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
        public required string Title { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1200, MinimumLength = 10, ErrorMessage = "Content chars must be between 10 - 1200 chars")]
        public required string Content { get; init; }

        [Required(ErrorMessage = "Media is required.")]
        [RegularExpression(@"^\d{17,19}$", ErrorMessage = "Media must contain only numbers and be between 17 and 19 characters.")]
        [Description("Media ID obtained in the path POST: /api/media/{category}")]
        public required string Media { get; init; }

        [Required(ErrorMessage = "BoardType is required.")]
        [EnumDataType(typeof(BoardTypes), ErrorMessage = "BoardType category.")]
        
        public BoardTypes BoardType { get; init; }

        [Description("Author nickname")]
        [DefaultValue("Anonymous")]
        public string? Author { get; init; }
        

    }

    
}

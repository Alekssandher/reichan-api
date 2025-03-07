using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using reichan_api.src.Enums;
using reichan_api.src.Utils;

namespace reichan_api.src.DTOs.Replies
{
    public class ReplyDto
    {
        [Required(ErrorMessage = "RepliesTo is required.")]
        [RegularExpression(@"^\d{16,19}$", ErrorMessage = "RepliesTo ID must be a valid numeric ID between 16 and 19 digits.")]
        public string RepliesTo { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1200, MinimumLength = 10, ErrorMessage = "Content chars must be between 10 - 1200 chars.")]
        public string Content { get; init; }

        [Required(ErrorMessage = "Media is required.")]
        [RegularExpression(@"^\d{16,19}$", ErrorMessage = "Media ID must be a valid numeric ID between 16 and 19 digits.")]
        [Description("Media ID obtained in the path POST: /api/media/{category}")]
        public string Media { get; init; }

        [Required(ErrorMessage = "Category is required.")]
        [EnumDataType(typeof(Categories), ErrorMessage = "Invalid category.")]
        public string Category { get; init; }

        [StringLength(30, MinimumLength = 1, ErrorMessage = "Author chars must be between 1 and 30.")]
        public string? Author { get; init; }

        [JsonIgnore]
        public string PublicId { get; init; }

        [JsonIgnore]
        public DateTime CreatedAt { get; init; }

        
        public ReplyDto(string repliesTo, string content, string media, string category, string? author)
        {
            RepliesTo = repliesTo;
            Content = content;
            Media = media;
            Category = category;
            Author = author ?? "Anonymous";
            PublicId = SnowflakeIdGenerator.GenerateId().ToString(); 
            CreatedAt = DateTime.UtcNow; 
        }
    }
}

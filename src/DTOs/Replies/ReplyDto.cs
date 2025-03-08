using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Replies
{
    public class ReplyDto
    {
        [Required(ErrorMessage = "ParentId is required.")]
        [RegularExpression(@"^\d{16,19}$", ErrorMessage = "RepliesTo ID must be a valid numeric ID between 16 and 19 digits.")]
        public required string ParentId { get; init; }

        [Required(ErrorMessage = "ParentType is required")]
        [EnumDataType(typeof(ParentType), ErrorMessage = "Invalid ParentType.")]
        public ParentType ParentType { get; init; }
        
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1200, MinimumLength = 10, ErrorMessage = "Content chars must be between 10 - 1200 chars.")]
        public required string Content { get; init; }

        [Required(ErrorMessage = "Media is required.")]
        [RegularExpression(@"^[a-z0-9/]+$", ErrorMessage = "Invalid image or video format.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Media lenght must be between 1 and 30.")]
        [Description("Media ID obtained in the path POST: /api/media/{category}")]
        public required string Media { get; init; }

        [StringLength(40, MinimumLength = 1, ErrorMessage = "Author chars must be between 1 and 30.")]
        public string? Author { get; init; }


    }
}

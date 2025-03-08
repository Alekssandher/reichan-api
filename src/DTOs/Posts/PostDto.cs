using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Posts
{
    public record class PostDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1200, MinimumLength = 10, ErrorMessage = "Content chars must be between 10 - 1200 chars")]
        public string Content { get; init; }

        [Required(ErrorMessage = "Media is required.")]
        [RegularExpression(@"^[a-z0-9/]+$", ErrorMessage = "Invalid image or video format.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Media id lenght invalid")]
        [Description("Media ID obtained in the path POST: /api/media/{category}")]
        public string Media { get; init; }

        [Required(ErrorMessage = "Category is required.")]
        [EnumDataType(typeof(Categories), ErrorMessage = "Invalid category.")]
        
        public string Category { get; init; }

        [Description("Author nickname")]
        [DefaultValue("Anonymous")]
        public string? Author { get; init; }
        
        [Description("Public Key of registred users")]
        [DefaultValue(null)]
        public string? AuthorPubKey { get; init; }

        [Description("Post signature.")]
        [DefaultValue(null)]
        public string? Signature { get; init; }

        [JsonIgnore]
        public DateTime CreatedAt { get; init; }

        [JsonIgnore]
        public bool Active { get; init; }

        [JsonIgnore]
        public int UpVotes { get; init; }
        
        [JsonIgnore]
        public int DownVotes { get; init; }


        public PostDto(
            string title, 
            string content, 
            string media, 
            string category, 
            string? author = "Anonymous", 
            string? authorPubKey = null, 
            string? signature = null
        )
            
        {
            Title = title;
            Content = content;
            Media = media;
            Category = category;
            Author = author ?? "Anonymous";
            AuthorPubKey = authorPubKey;
            Signature = signature;
            CreatedAt = DateTime.UtcNow;
            Active = true;
            UpVotes = 0;
            DownVotes = 0;
        }
    }

    
}

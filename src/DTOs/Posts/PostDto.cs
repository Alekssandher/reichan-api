using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace reichan_api.src.DTOs.Posts
{
    public record class PostDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(600, MinimumLength = 1, ErrorMessage = "Content chars must be between 1 - 600 chars")]
        [DataType(DataType.Text)]
        public string Content { get; init; }

        [Required(ErrorMessage = "Image is required.")]
        [RegularExpression(@"^[\w,\s-]+\.(jpg|jpeg|png|gif|webpm|mp4|ogg)$", ErrorMessage = "Invalid image or video format.")]
        public string Image { get; init; }

        [Required(ErrorMessage = "Category is required.")]
        [EnumDataType(typeof(PostCategory), ErrorMessage = "Invalid category.")]
        public string Category { get; init; }

        public string? Author { get; init; }
        public string? AuthorPubKey { get; init; }
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
            string image, 
            string category, 
            string? author = "Anonymous", 
            string? authorPubKey = null, 
            string? signature = null
        )
            
        {
            Title = title;
            Content = content;
            Image = image;
            Category = category;
            Author = author;
            AuthorPubKey = authorPubKey;
            Signature = signature;
            CreatedAt = DateTime.UtcNow;
            Active = true;
            UpVotes = 0;
            DownVotes = 0;
        }
    }

    public enum PostCategory
    {
        news,
        blog,
        tutorial,
        review
    }
}

using MongoDB.Bson;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Models.Posts;
using reichan_api.src.Utils;

namespace reichan_api.src.Mappers {
    public static class PostMapper
    {
        public static PostResponseDTO ResponseToDto(this PostModel post)
        {
            return new PostResponseDTO
            {
                Id = post.PublicId,
                Author = post.Author,
                Title = post.Title,
                Content = post.Content,
                Media = post.Media,
                Category = post.Category,
                UpVotes = post.UpVotes,
                DownVotes = post.DownVotes,
                CreatedAt = post.CreatedAt,
                Active = post.Active                
            };
        }

        public static PostModel ToModel(this PostDto postDto ) {
            
            return new PostModel 
            {
                Author = postDto.Author ?? "Anonymous",
                PublicId = SnowflakeIdGenerator.GenerateId().ToString(),
                Title = postDto.Title,
                Content = postDto.Content,
                Media = postDto.Media,
                Category = postDto.Category,
                UpVotes = 0,
                DownVotes = 0,
                CreatedAt = DateTime.UtcNow,
                Active = true
            };
        }
    }

}
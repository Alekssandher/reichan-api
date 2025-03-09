using reichan_api.src.DTOs.Threads;
using reichan_api.src.Models.Threads;
using reichan_api.src.Utils;

namespace reichan_api.src.Mappers {
    public static class ThreadMapper
    {
        public static ThreadResponseDto ResponseToDto(this ThreadModel post)
        {
            return new ThreadResponseDto
            {
                Id = post.PublicId,
                Author = post.Author,
                Title = post.Title,
                Content = post.Content,
                Media = post.Media,
                BoardType = post.BoardType,
                UpVotes = post.UpVotes,
                DownVotes = post.DownVotes,
                CreatedAt = post.CreatedAt,
                Active = post.Active                
            };
        }

        public static ThreadModel ToModel(this ThreadDto postDto ) {
            
            return new ThreadModel 
            {
                Author = postDto.Author ?? "Anonymous",
                PublicId = SnowflakeIdGenerator.GenerateId().ToString(),
                Title = postDto.Title,
                Content = postDto.Content,
                Media = postDto.Media,
                BoardType = postDto.BoardType,
                UpVotes = 0,
                DownVotes = 0,
                CreatedAt = DateTime.UtcNow,
                Active = true
            };
        }
    }

}
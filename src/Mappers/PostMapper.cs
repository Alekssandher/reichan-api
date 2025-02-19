using MongoDB.Bson;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Models.Posts;

namespace reichan_api.src.Mappers {
    public static class PostMapper
    {
        public static PostResponseDTO ResponseToDto(this PostModel post)
        {
            return new PostResponseDTO
            {
                Id = post.PublicId!,
                AuthorPubKey = post.AuthorPubKey,
                Author = post.Author,
                Title = post.Title,
                Content = post.Content,
                Media = post.Media,
                Category = post.Category,
                Signature = post.Signature,
                UpVotes = post.UpVotes,
                DownVotes = post.DownVotes,
                CreatedAt = post.CreatedAt,
                Active = post.Active                
            };
        }

        public static PostModel ResponseToModel(this PostResponseDTO postResponseDto)
        {
            return new PostModel
            {
                PublicId = postResponseDto.Id,
                AuthorPubKey = postResponseDto.AuthorPubKey,
                Author = postResponseDto.Author,
                Title = postResponseDto.Title,
                Content = postResponseDto.Content,
                Media = postResponseDto.Media,
                Category = postResponseDto.Category,
                Signature = postResponseDto.Signature,
                UpVotes = postResponseDto.UpVotes,
                DownVotes = postResponseDto.DownVotes,
                CreatedAt = postResponseDto.CreatedAt
            };
        }

        public static PostModel ToModel(this PostDto postDto ) {
            
            return new PostModel 
            {
                AuthorPubKey = postDto.AuthorPubKey,
                Author = postDto.Author!,
                Title = postDto.Title!,
                Content = postDto.Content!,
                Media = postDto.Media!,
                Category = postDto.Category!.ToLower(),
                Signature = postDto.Signature,
                UpVotes = postDto.UpVotes,
                DownVotes = postDto.DownVotes,
                CreatedAt = postDto.CreatedAt
            };
        }
    }

}
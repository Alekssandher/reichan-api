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
                Id = post.Id!,
                AuthorPubKey = post.AuthorPubKey,
                Author = post.Author,
                Title = post.Title,
                Content = post.Content,
                Image = post.Image,
                Category = post.Category,
                Signature = post.Signature,
                UpVotes = post.UpVotes,
                DownVotes = post.DownVotes
            };
        }

        public static PostModel ResponseToModel(this PostResponseDTO postResponseDto)
        {
            return new PostModel
            {
                Id = postResponseDto.Id,
                AuthorPubKey = postResponseDto.AuthorPubKey,
                Author = postResponseDto.Author,
                Title = postResponseDto.Title,
                Content = postResponseDto.Content,
                Image = postResponseDto.Image,
                Category = postResponseDto.Category,
                Signature = postResponseDto.Signature,
                UpVotes = postResponseDto.UpVotes,
                DownVotes = postResponseDto.DownVotes
            };
        }

        public static PostModel ToModel(this PostDto postDto ) {
            
            return new PostModel 
            {
                AuthorPubKey = postDto.AuthorPubKey,
                Author = postDto.Author,
                Title = postDto.Title!,
                Content = postDto.Content!,
                Image = postDto.Image!,
                Category = postDto.Category!,
                Signature = postDto.Signature,
                UpVotes = postDto.UpVotes,
                DownVotes = postDto.DownVotes
            };
        }
    }

}
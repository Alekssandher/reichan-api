using ReichanApi.DTOs;
using ReichanApi.Models;

namespace ReichanApi.Mappers {
    public static class PostMapper
    {
        public static PostResponseDTO ToDto(this PostModel post)
        {
            return new PostResponseDTO
            {
                Id = post.Id,
                AuthorPubKey = post.AuthorPubKey,
                Author = post.Author,
                Title = post.Title,
                Content = post.Content,
                Image = post.Image,
                Category = post.Category,
                Signature = post.Signature,
                Upvotes = post.UpVotes,
                DownVotes = post.DownVotes
            };
        }

        public static PostModel ToModel(this PostResponseDTO postDto)
        {
            return new PostModel
            {
                Id = postDto.Id,
                AuthorPubKey = postDto.AuthorPubKey,
                Author = postDto.Author,
                Title = postDto.Title,
                Content = postDto.Content,
                Image = postDto.Image,
                Category = postDto.Category,
                Signature = postDto.Signature,
                UpVotes = postDto.Upvotes,
                DownVotes = postDto.DownVotes
            };
        }
    }

}
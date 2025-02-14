using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.Interfaces;
using reichan_api.src.Mappers;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Modules.Posts {
    public class PostsService : IPostService
    {
        private readonly IPostRepository _postsRepository;

        public PostsService(IPostRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<IReadOnlyList<PostResponseDTO>> GetAllAsync( PostQueryParams queryParams)
        {
            var posts = await _postsRepository.GetAllAsync(queryParams);
            return posts.Select(post => post.ResponseToDto()).ToList();
        }

        public async Task<PostResponseDTO?> GetByIdAsync(string id)
        {
            var post = await _postsRepository.GetByIdAsync(id);
            return post?.ResponseToDto();
        }

        public async Task<bool> VoteAsync(string id, bool vote)
        {
            return await _postsRepository.UpdateVoteAsync(id, vote);
        }

        public async Task<bool> CreateAsync(PostDto postDto)
        {
            return await _postsRepository.InsertAsync(postDto.ToModel());
        }
    }

}
using MongoDB.Driver;
using reichan_api.src.Interfaces;
using reichan_api.src.Mappers;
using reichan_api.src.QueryParams;
using reichan_api.src.Models.Threads;
using reichan_api.src.DTOs.Threads;

namespace reichan_api.src.Modules.Threads {
    public class ThreadsService : IThreadService
    {
        private readonly IThreadRepository _threadsRepository;

        public ThreadsService(IThreadRepository threadsRepository)
        {
            _threadsRepository = threadsRepository;
        }

        public async Task<IReadOnlyList<ThreadResponseDto>> GetAllAsync( ThreadQueryParams queryParams )
        {
            IReadOnlyList<ThreadModel> threads = await _threadsRepository.GetAllAsync(queryParams);
            return threads.Select(thread => thread.ResponseToDto()).ToList();
        }

        public async Task<ThreadResponseDto?> GetByIdAsync(string id)
        {
            ThreadModel? thread = await _threadsRepository.GetByIdAsync(id);
            return thread?.ResponseToDto();
        }

        public async Task<bool> VoteAsync(string id, bool vote)
        {
            return await _threadsRepository.UpdateVoteAsync(id, vote);
        }

        public async Task<bool> CreateAsync(ThreadDto threadDto)
        {
            return await _threadsRepository.InsertAsync(threadDto.ToModel());
        }
    }

}
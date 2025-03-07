using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reichan_api.src.Models.Replies;

namespace reichan_api.src.Interfaces.replies
{    
    public interface IReplyRepository
    {
        Task<IReadOnlyList<ReplyModel>> GetAllAsync();
        // Task<ReplyModel?> GetByIdAsync(string id);
        // Task<bool> UpdateVoteAsync(string id, bool vote);
        Task<bool> InsertAsync(ReplyModel reply);
    }
}
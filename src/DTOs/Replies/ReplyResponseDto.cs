using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reichan_api.src.DTOs.Replies
{
    public class ReplyResponseDto
    {
        public required string Id { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required string Category { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
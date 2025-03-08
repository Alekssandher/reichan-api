using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Replies
{
    public class ReplyResponseDto
    {
        public required string Id { get; init; }
        public required string ParentId { get; init; }
        public required ParentType ParentType { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
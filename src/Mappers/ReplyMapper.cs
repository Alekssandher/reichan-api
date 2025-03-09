using reichan_api.src.DTOs.Replies;
using reichan_api.src.Models.Replies;
using reichan_api.src.Utils;

namespace reichan_api.src.Mappers
{
    public static class ReplyMapper
    {
        public static ReplyResponseDto ToResponseDto(this ReplyModel model)
        {
            return new ReplyResponseDto {
                Id = model.PublicId,
                ParentId = model.ParentId,
                BoardType = model.BoardType,
                ParentType = model.ParentType,
                Content = model.Content,
                Media = model.Media,
                Author = model.Author,
                CreatedAt = model.CreatedAt
            };
        }
        public static ReplyModel ToModel(this ReplyDto dto)
        {
            return new ReplyModel {
                ParentId = dto.ParentId,
                ParentType = dto.ParentType,
                PublicId = SnowflakeIdGenerator.GenerateId().ToString(),
                BoardType = dto.BoardType,
                Content = dto.Content,
                Media = dto.Media,
                Author = dto.Author ?? "Anonymous",
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
using reichan_api.src.DTOs.Replies;
using reichan_api.src.Models.Replies;

namespace reichan_api.src.Mappers
{
    public static class ReplyMapper
    {
        public static ReplyResponseDto ToResponseDto(this ReplyModel model)
        {
            return new ReplyResponseDto {
                Id = model.PublicId,
                Content = model.Content,
                Media = model.Media,
                Category = model.Category,
                Author = model.Author,
                CreatedAt = model.CreatedAt
            };
        }
        public static ReplyModel ToModel(this ReplyDto dto)
        {
            return new ReplyModel {
                PublicId = dto.PublicId,
                RepliesTo = dto.RepliesTo,
                Content = dto.Content,
                Media = dto.Media,
                Category = dto.Category,
                Author = dto.Author!,
                CreatedAt = dto.CreatedAt
            };
        }
    }
}
namespace reichan_api.src.DTOs.Posts {
    public class PostResponseWrapperDTO
    {
        public required byte Status { get; set; }
        public required PostResponseDTO Post { get; set; }
    }
}
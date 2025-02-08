namespace ReichanApi.DTOs {
    public class PostResponseWrapperDTO
    {
        public required byte Status { get; set; }
        public required PostResponseDTO Post { get; set; }
    }
}
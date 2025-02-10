namespace reichan_api.src.DTOs.Global {
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public T? Data { get; set; }
    }
}
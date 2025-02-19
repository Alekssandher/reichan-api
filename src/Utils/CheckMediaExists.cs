using reichan_api.src.DTOs.Posts;

namespace reichan_api.src.Utils
{
    public class CheckMediaExists
    {
        private static readonly string cloudiUrl = "https://res.cloudinary.com/dnf22gtjt/image/upload/f_auto,q_auto/v1";
        private static readonly HttpClient httpClient = new();
        public static async Task<bool> CheckImageExistsAsync(PostDto postDto)
        {
            

            if(string.IsNullOrEmpty(postDto.Category) || string.IsNullOrEmpty(postDto.Media))
            {
                return false;
            }
            string completedUrl = $"{cloudiUrl}/{postDto.Category}/{postDto.Media}";

            Console.WriteLine(completedUrl);
            HttpResponseMessage response = await httpClient.GetAsync(completedUrl);

            if(!response.IsSuccessStatusCode) return false;

            return true;
            
            
        }
    }
}
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Enums;

namespace reichan_api.src.Utils
{
    public class CheckMediaExists
    {
        private static readonly string cloudiUrl = "https://res.cloudinary.com/dnf22gtjt/image/upload/v1739916746";
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
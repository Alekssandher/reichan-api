using reichan_api.src.DTOs.Posts;

namespace reichan_api.src.Utils
{
    public class CheckMediaExists
    {
        private static readonly string cloudiUrl = "https://res.cloudinary.com/dnf22gtjt/image/upload/f_auto,q_auto/v1";
        private static readonly HttpClient httpClient = new();
        public static async Task<bool> CheckImageExistsAsync(string media, string category)
        {
            

            if(string.IsNullOrEmpty(category) || string.IsNullOrEmpty(media))
            {
                return false;
            }
            string completedUrl = $"{cloudiUrl}/{category}/{media}";

            HttpResponseMessage response = await httpClient.GetAsync(completedUrl);

            if(!response.IsSuccessStatusCode) return false;

            return true;
            
            
        }
    }
}
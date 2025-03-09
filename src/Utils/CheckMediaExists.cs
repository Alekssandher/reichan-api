using reichan_api.src.DTOs.Threads;
using reichan_api.src.Enums;

namespace reichan_api.src.Utils
{
    public class CheckMediaExists
    {
        private static readonly string cloudiUrl = "https://res.cloudinary.com/dnf22gtjt/image/upload/f_auto,q_auto/v1";
        private static readonly HttpClient httpClient = new();
        public static async Task<bool> CheckImageExistsAsync(string media, BoardTypes boardType)
        {
            

            if(string.IsNullOrEmpty(media))
            {
                return false;
            }
            if(!Enum.IsDefined(typeof(BoardTypes), boardType))
            {
                return false;
            }

            string completedUrl = $"{cloudiUrl}/{boardType}/{media}";

            HttpResponseMessage response = await httpClient.GetAsync(completedUrl);

            if(!response.IsSuccessStatusCode) return false;

            return true;
            
            
        }
    }
}
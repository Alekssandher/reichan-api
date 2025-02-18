
namespace reichan_api.src.Utils
{
    public static class ImageChecker
    {
        private static readonly Dictionary<string, bool> _cache = new();
        private static readonly string clientId =  Environment.GetEnvironmentVariable("IMGUR_CLIENT_ID") ?? "";
        public static async Task<bool> CheckImageExists(string mediaId)
        {
            if (_cache.TryGetValue(mediaId, out bool exists)) 
                return exists; 

            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", clientId);
            
            var response = await client.GetAsync("https://api.imgur.com/3/image/" + mediaId);
            bool found = response.IsSuccessStatusCode;

            _cache[mediaId] = found; 
            return found;
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}

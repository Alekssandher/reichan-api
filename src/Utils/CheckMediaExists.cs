using reichan_api.src.DTOs.Posts;

namespace reichan_api.src.Utils
{
    public class CheckMediaExists
    {
        public static bool CheckImageExists(PostDto body)
        {
            if(string.IsNullOrEmpty(body.Media) || string.IsNullOrEmpty(body.Category.ToString()))
            {
                return false;
            }
            var filePath = Path.Combine("../storage/uploads", body.Category.ToString(), body.Media);

            if (!File.Exists(filePath))
            {
                return false;
            }

            return true;
        }
    }
}
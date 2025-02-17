using reichan_api.src.DTOs.Posts;
using reichan_api.src.Enums;

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
            string strCategory = Enum.GetName(typeof(PostCategory), body.Category)!;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", strCategory.ToLower(), body.Media);

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine(filePath);
                return false;
            }

            return true;
        }
    }
}
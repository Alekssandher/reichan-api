using Microsoft.AspNetCore.Http.HttpResults;

public class CheckImage
{
    public static bool CheckImageExists(CreateReplyDto body)
    {
        if(string.IsNullOrEmpty(body.Image) || string.IsNullOrEmpty(body.Category))
        {
            return false;
        }
        var filePath = Path.Combine("storage/uploads", body.Category, body.Image);

        if (!File.Exists(filePath))
        {
            return false;
        }

        return true;
    }
}
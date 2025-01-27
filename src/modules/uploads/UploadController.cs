using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]

public class UploadController : ControllerBase {
    // private readonly IPostService _postService;

    // public UploadImageController(IPostService postService)
    // {
    //     _postService = postService;
    // }

    [HttpPost("image")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string category)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { success = false, message = "Invalid file." });
        }

        var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!permittedMimeTypes.Contains(file.ContentType) || !permittedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { success = false, message = "Only image files are allowed." });
        }

        if (string.IsNullOrEmpty(category))
        {
            return BadRequest(new { success = false, message = "Invalid category." });
        }

        var fileId = Guid.NewGuid().ToString();

        var categoryPath = Path.Combine("storage/images", category);
        if (!Directory.Exists(categoryPath))
        {
            Directory.CreateDirectory(categoryPath);
        }

        var filePath = Path.Combine(categoryPath, fileId + Path.GetExtension(file.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var fileName = fileId + Path.GetExtension(file.FileName);

        return Ok(new { success = true, fileName, category });
    }


}
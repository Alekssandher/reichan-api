using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]

public class ImagesController : ControllerBase {
    // private readonly IPostService _postService;

    // public UploadImageController(IPostService postService)
    // {
    //     _postService = postService;
    // }
    

    [HttpPost("upload")]
    [ServiceFilter(typeof(ValidateCategory))]
    [RequestSizeLimit(3 * 1024 * 1024)] // 3 MB
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string category)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { success = false, message = "Invalid file." });
        }

        string[] permittedMimeTypes = 
        [
            "image/jpeg", "image/png", "image/gif",   // Imagens
            "video/mp4", "video/webm", "video/avi"   // Vídeos
        ];

        string[] permittedExtensions = 
        [
            ".jpg", ".jpeg", ".png", ".gif",         // Imagens
            ".mp4", ".webm", ".avi"                 // Vídeos
        ];

        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!permittedMimeTypes.Contains(file.ContentType) || !permittedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { success = false, message = "Only image files are allowed." });
        }

        if (string.IsNullOrEmpty(category))
        {
            return BadRequest(new { success = false, message = "Invalid category." });
        }

        string fileId = Guid.NewGuid().ToString();

        string categoryPath = Path.Combine("storage/uploads", category);

        if (!Directory.Exists(categoryPath))
        {
            Directory.CreateDirectory(categoryPath);
        }

        string filePath = Path.Combine(categoryPath, fileId + Path.GetExtension(file.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        string fileName = fileId + Path.GetExtension(file.FileName);

        return Ok(new { success = true, fileName, category });
    }

    [HttpGet("get/{category}/{fileName}")]
    [RequestSizeLimit(3 * 1024 * 1024)] // 3 MB
    public IActionResult GetImage(string category, string fileName)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "storage", "uploads", category.ToLower(), fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound(new { success = false, message = "File not found." });

        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        // Define MIME type
        string fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
        string mimeType = fileExtension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream" // Generical MIME type
        };

        return File(fileBytes, mimeType);
    }

}
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using reichan_api.Filters;
using reichan_api.src.DTOs.Global;
using reichan_api.src.Enums;
using reichan_api.src.Utils;

namespace reichan_api.src.Modules.Medias
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase 
    {
        private readonly BadRequest InvalidFileError = new("Invalid File", "The file provided is null or malformed.");
        private readonly BadRequest InvalidFileTypeError = new ("Invalid File Type ", "Only image or video files are allowed.");
        private readonly BadRequest InvalidCategoryError = new("Invalid Category", "You must provide a valid category.");

        [HttpPost("{category}")]
        //[RequestSizeLimit( 3 * 1024 * 1024)] // 3 MB

        [EndpointName("UploadMedia")]
        [EndpointSummary("UploadMedia")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> UploadFile(

            [Required(ErrorMessage = "File is required.")]
            [DataType(DataType.Upload)]

            IFormFile file, 
            
            [Required(ErrorMessage = "Category is required.")]
            [EnumDataType(typeof(PostCategory), ErrorMessage = "You must provide a valid category.")]
            [FromRoute] 
            PostCategory category 
        )
        
        {
            if ( file == null || file.Length == 0) return BadRequest( InvalidFileError );

            string[] permittedMimeTypes = 
            [
                "image/jpeg", "image/png", "image/webp", "image/gif",   
                "video/mp4", "video/webm", "video/avi"
            ];

            string[] permittedExtensions = 
            [
                ".jpg", ".jpeg", ".png", ".webp", ".gif",         
                ".mp4", ".webm", ".avi"                 
            ];

            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!permittedMimeTypes.Contains(file.ContentType) || !permittedExtensions.Contains(fileExtension)) return BadRequest( InvalidFileTypeError );
            
            string strCategory = category.ToString();

            if( string.IsNullOrEmpty(strCategory)) return BadRequest( InvalidCategoryError );

            string categoryPath = Path.Combine("wwwroot/uploads", strCategory);

            if(!Directory.Exists(categoryPath)) Directory.CreateDirectory(categoryPath);

            string fileId = SnowflakeIdGenerator.GenerateId().ToString();

            string filePath = Path.Combine(categoryPath, fileId + Path.GetExtension(file.FileName));

           
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string fileName = fileId + Path.GetExtension(file.FileName);
        
            return Ok(new { success = true, fileName, category });
        }

        [HttpGet("{category}/{fileName}")]
        [RequestSizeLimit(3 * 1024 * 1024)] // 3 MB
        [ServiceFilter(typeof(ValidateGetMedia))]
        // Documentation
        [EndpointName("GetMedia")]
        [EndpointSummary("GetMedia")]

        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/kind+extension")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        
        public IActionResult GetImage( 
            [Required(ErrorMessage = "Category is required.")]
            [EnumDataType(typeof(PostCategory), ErrorMessage = "You must provide a valid category.")]
            [FromRoute] 
            PostCategory category, 

            [FromRoute]
            string fileName
            )
        {
            string strCategory = category.ToString();
            if (!Enum.TryParse<PostCategory>(strCategory, true, out var categoryEnum) || !Enum.IsDefined(categoryEnum))
            {
                return BadRequest( InvalidCategoryError );
            }
            
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", strCategory.ToLower(), fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new NotFound("File Not Found", "Check the name and category of the file.") );

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // MIME type
            string fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
            string mimeType = fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".gif" => "image/gif",

                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".avi" => "video/x-msvideo",
                ".mov" => "video/quicktime",
                ".mkv" => "video/x-matroska",

                _ => "application/octet-stream" // Generical MIME type
            };


            return File(fileBytes, mimeType);
        }

    }
    
}
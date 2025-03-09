using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using reichan_api.Filters.captcha;
using reichan_api.src.DTOs.Responses;
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
        private readonly BadRequest InvalidBoardTypeError = new("Invalid BoardType", "You must provide a valid BoardType.");
        private readonly Cloudinary _cloudinary;
        public MediasController(Cloudinary cloudinary) {
            _cloudinary = cloudinary;
        }

        [HttpPost("{boardType}")]
        [RequestSizeLimit( 3 * 1024 * 1024)] // 3 MB
        [ServiceFilter(typeof(ValidateCaptcha))]

        [EndpointName("UploadMedia")]
        [EndpointSummary("UploadMedia")]
        [ProducesResponseType(typeof(OkResponse<string>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> UploadFile(

            [Required(ErrorMessage = "File is required.")]
            [DataType(DataType.Upload)]

            IFormFile file, 

            [Required(ErrorMessage = "BoardType is required.")]
            [EnumDataType(typeof(BoardTypes), ErrorMessage = "You must provide a valid BoardType.")]
            [FromRoute] 
            BoardTypes boardType,
            
            [FromHeader(Name = "X-CaptchaCode")] 
            string CaptchaCode
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
            
            string strBoardType = boardType.ToString();

            if( string.IsNullOrEmpty(strBoardType)) return BadRequest( InvalidBoardTypeError );

            using var stream = file.OpenReadStream();
            
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(file.FileName, stream),
                UseFilename = true,
                Folder = strBoardType,
                PublicId = SnowflakeIdGenerator.GenerateId().ToString(),
                UniqueFilename = false,
                Overwrite = true
            };
            
            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            string fullId = uploadResult.PublicId; 
            string fileId = fullId.Substring(fullId.LastIndexOf('/') + 1); 

            return Ok(new OkResponse<string>("Uploaded", "File was uploaded successfully", fileId));


        }

        
    }
    
}
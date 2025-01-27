using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]

public class PostsController : ControllerBase {
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("findAll")]
    public async Task<IActionResult> GetPosts([FromQuery] PostQueryParams queryParams)
    {
        var posts = await _postService.GetAllAsync(queryParams);
        
        return Ok(new {
            success = true,
            posts.Posts
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto body)
    {
        try
        {
            var filePath = Path.Combine("storage/images", body.Category, body.Image);

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest(new { success = false, message = "Arquivo não encontrado na categoria fornecida." });
            }

            await _postService.CreateAsync(body);
            return Ok("Created");
        }
        catch (Exception ex)
        {
            throw new Exception("Exeption ocurred." + ex.Message);
        }
    }
    
    [ServiceFilter(typeof(ValidateSignature))]
    [HttpPost("createSigned")]
    public async Task<IActionResult> CreatePost([FromBody] CreateSignedPostDto body)
    {
        try
        {            
            await _postService.CreateSignedAsync(body);
            return Ok("Created");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            throw new Exception("Exeption ocurred." + ex.Message);
        }
        
    }
}
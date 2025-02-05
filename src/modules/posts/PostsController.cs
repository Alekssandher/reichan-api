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
    [HttpGet("find/{id}")]
    public async Task<IActionResult> GetPostById(string id)
    {
        var post = await _postService.GetByIdAsync(id);
        return Ok(new { success = true, post });
    }

    [HttpPost("vote/{id}/{kindVote}")]
    [ServiceFilter(typeof(ValidateCaptcha))]
    public async Task<IActionResult> VotePost(string id, bool kindVote) {

        try
        {
            int vote = kindVote ? 1 : -1;

            await _postService.VotePostAsync(vote, id);
            return Ok("Voted");
        }
        catch (InvalidOperationException ex) 
        {
            return StatusCode(400, new {succes = false, message = ex.Message});
        }
        catch (KeyNotFoundException ex) 
        {
            return BadRequest( new {succes = false, message = ex.Message});
        }
        catch (Exception)
        {
            
            return StatusCode(500, new {succes = false, message = "Internal error."});
        }
    }

    [HttpPost("create")]
    [ServiceFilter(typeof(ValidateCategoryPost))]
    [ServiceFilter(typeof(ValidateCaptcha))]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto body)
    {
        try
        {
            if(string.IsNullOrEmpty(body.Author))
            {
                body.Author = "Anonymous";
            }

            var baseDirectory = Path.Combine("storage", "uploads", body.Category);
            var filePath = Path.Combine(baseDirectory, body.Image);

            
            var fullPath = Path.GetFullPath(filePath);
           
            if (!fullPath.EndsWith(filePath))
            {
                return BadRequest(new { success = false, message = "Invalid file path." });
            }

            if (!System.IO.File.Exists(fullPath))
            {
                return BadRequest(new { success = false, message = "Image not found, make sure to send it first." });
            }

            await _postService.CreateAsync(body);
            return Ok("Created");
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
        }
    }
    
    [ServiceFilter(typeof(ValidateSignature))]
    [ServiceFilter(typeof(ValidateCategoryPost))]
    [HttpPost("createSigned")]
    public async Task<IActionResult> CreatePost([FromBody] CreateSignedPostDto body)
    {
        try
        {            
            await _postService.CreateSignedAsync(body);
            return Ok("Created");
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
        }
        
    }
}
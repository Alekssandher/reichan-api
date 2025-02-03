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

        int vote = kindVote ? 1 : -1;

        await _postService.VotePostAsync(vote, id);
        return Ok("Voted");
    }

    [HttpPost("create")]
    [ServiceFilter(typeof(ValidateCategoryPost))]

    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto body)
    {
        try
        {
            if(string.IsNullOrEmpty(body.Author))
            {
                body.Author = "Anonymous";
            }
            var filePath = Path.Combine("storage/uploads", body.Category, body.Image);

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest(new { success = false, message = "Image not found, make sure to send it first." });
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
    [ServiceFilter(typeof(ValidateCategoryPost))]
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
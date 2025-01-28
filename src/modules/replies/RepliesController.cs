using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class RepliesController : ControllerBase {
    private readonly IReplyService _replyService;

    public RepliesController(IReplyService replyService)
    {
        _replyService = replyService;
    }

    [HttpGet("findAll")]
    public async Task<IActionResult> GetReplies()
    {
        var replies = await _replyService.GetAllAsync();
        
        return Ok(new {
            success = true,
            replies.Replies
        });
    }

    [HttpPost("create/{targetId}")]
    public async Task<IActionResult> CreatePost([FromBody] CreateReplyDto body, [FromRoute] string targetId)
    {
        try
        {
            body.PostId = targetId;
           
            await _replyService.CreateAsync(body, targetId);
            return Ok("Created");
        }
        catch (Exception ex)
        {
            throw new Exception("Exeption ocurred: " + ex.Message);
        }
    }
    
    // [ServiceFilter(typeof(ValidateSignature))]
    // [HttpPost("createSigned")]
    // public async Task<IActionResult> CreatePost([FromBody] CreateSignedPostDto body)
    // {
    //     try
    //     {            
    //         await _postService.CreateSignedAsync(body);
    //         return Ok("Created");
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         return BadRequest(new { success = false, message = ex.Message });
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception("Exeption ocurred." + ex.Message);
    //     }
        
    // }
}
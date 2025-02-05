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
    public async Task<IActionResult> GetReplies([FromQuery] ReplyQueryParams queryParams)
    {
        var replies = await _replyService.GetAllAsync(queryParams);
        
        return Ok(new {
            success = true,
            replies
        });
    }

    [HttpGet("find/{id}")]
    public async Task<IActionResult> GetReply([FromRoute] string id)
    {
        try
        {
            var reply = await _replyService.GetByIdAsync(id);
            return Ok(new { success = true, reply.Replies });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
    [HttpPost("createToReply/{targetId}")]
    [ServiceFilter(typeof(ValidateCaptcha))]
    public async Task<IActionResult> CreateToReplyPost([FromBody] CreateReplyDto body, [FromRoute] string targetId) 
    {
        try
        {
            Console.WriteLine("Arrived controller");
            if(string.IsNullOrEmpty(body.Author))
            {
                body.Author = "Anonymous";
            }
            if(!string.IsNullOrEmpty(body.Image))
            {
                if (CheckImage.CheckImageExists(body) == false)
                {
                    return BadRequest(new { success = false, message = "Image does not exist" });
                }
            }
            else
            {
                body.Image = null;
            }
            

            body.RepliesTo = targetId;
           
            await _replyService.CreateToReplyAsync(body, targetId);
            return Ok("Created");
        }
        catch (Exception ex)
        {
            
            throw new Exception("Exeption ocurred: " + ex.Message);
        }
    }

    [HttpPost("create/{targetId}")]
    public async Task<IActionResult> CreatePost([FromBody] CreateReplyDto body, [FromRoute] string targetId)
    {
        try
        {
            if(string.IsNullOrEmpty(body.Author))
            {
                body.Author = "Anonymous";
            }
            if(!string.IsNullOrEmpty(body.Image))
            {
                if (CheckImage.CheckImageExists(body) == false)
                {
                    return BadRequest(new { success = false, message = "Image does not exist" });
                }
            }
            else
            {
                body.Image = null;
            }
            

            body.RepliesTo = targetId;
           
            await _replyService.CreateAsync(body, targetId);
            return Ok("Created");
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
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
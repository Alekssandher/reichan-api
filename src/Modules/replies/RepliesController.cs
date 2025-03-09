using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using reichan_api.filters.threads;
using reichan_api.Filters.captcha;
using reichan_api.src.DTOs.Replies;
using reichan_api.src.DTOs.Responses;
using reichan_api.src.Enums;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.QueryParams;
using reichan_api.src.Utils;

namespace reichan_api.src.Modules.replies
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepliesController : ControllerBase
    {
        private readonly NotFound notFound = new("Not Found", "We couldn't find the thread or reply you are refering to.");
        private readonly CreatedResponse createdResponse = new();
        private readonly IReplyService _replyService;
        public RepliesController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpGet]
       
        [Consumes("application/json")]  
        [EndpointName("GetReplies")]
        [EndpointSummary("GetReplies")]
        [EndpointDescription("Get all replies.")]
        [ProducesResponseType(typeof(OkResponse<ReplyResponseDto>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> GetReplies([FromQuery, Required] BoardTypes boardType, [FromQuery] ReplyQueryParams replyQuery)
        {
            IReadOnlyList<ReplyResponseDto> replies = await _replyService.GetAllAsync(boardType, replyQuery);

            return StatusCode(200, new OkResponse<IReadOnlyList<ReplyResponseDto>>("Replies Found", "Replies fetched successfuly.", replies));

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCaptcha))]
        [Consumes("application/json")]  
        [EndpointName("CreateReply")]
        [EndpointSummary("CreateReply")]
        [EndpointDescription("Reply a post by its id.")]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        public async Task<ActionResult> CreateReply([FromBody] ReplyDto reply, [FromHeader(Name = "X-CaptchaCode")] string CaptchaCode)
        {   
            if(reply.Media != null)
            {
                bool mediaExists = await CheckMediaExists.CheckImageExistsAsync(reply.Media, reply.BoardType);
            
                if(!mediaExists) return NotFound(new NotFound("Not Found","The media provided does not exist or was not found."));

            }
            
            bool created = await _replyService.CreateAsync(reply);

            return created
                ? StatusCode(201, createdResponse)
                : StatusCode(404, notFound);
        }
    }
}
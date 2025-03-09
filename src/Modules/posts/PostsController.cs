using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using reichan_api.src.QueryParams;
using reichan_api.src.Interfaces;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.DTOs.Responses;
using reichan_api.Filters;
using reichan_api.src.Utils;
using reichan_api.filters.Posts;
using reichan_api.Filters.captcha;

namespace reichan_api.src.Modules.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostService _threadService;
        private readonly ILogger<PostsController> _logger;

        // Responses objects
        private readonly InternalError internalError = new();
        private readonly NotFound threadNotFound = new("Not Found", "There are no threads mathing the query.");
        private readonly NoContentResponse noContentResponse = new();
        private readonly CreatedResponse createdResponse = new();

        public PostsController(IPostService threadService, ILogger<PostsController> logger)
        {
            _threadService = threadService;
            _logger = logger;
            
        }

        
        [HttpGet]
        [ServiceFilter(typeof(ValidateQueryAttribute))]
        
        // Documentation
        [EndpointName("GetPosts")]
        [EndpointSummary("GetPosts")]
        [EndpointDescription("Retrieves a list of threads based on the provided query parameters.")]
        [ProducesResponseType(typeof(OkResponse<IReadOnlyList<ThreadResponseDto>>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        
        public async Task<ActionResult> GetPosts([FromQuery] PostQueryParams queryParams)
        {    
            
            IReadOnlyList<ThreadResponseDto> threads = await _threadService.GetAllAsync(queryParams);
            
            if (!threads.Any()) return NotFound( threadNotFound );
            
            return Ok(new OkResponse<IReadOnlyList<ThreadResponseDto>>("Posts Found.", "Posts fetched successfuly.", threads));

            
            
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdAttribute))]
        // Documentation
        [EndpointName("GetPostById")]
        [EndpointSummary("GetPostById")]
        [EndpointDescription("Retrieves a thread based on the provided ID.")]
        [ProducesResponseType(typeof(OkResponse<ThreadResponseDto>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ContentTooLarge), StatusCodes.Status413RequestEntityTooLarge, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        
        public async Task<ActionResult> GetPostById( [FromRoute] string id ) {

            ThreadResponseDto? thread = await _threadService.GetByIdAsync(id);

            if (thread == null) return NotFound(new NotFound("Thread Not Found", $"Post with ID '{id}' was not found."));
             
            return Ok(
                new OkResponse<ThreadResponseDto>
                (
                    "Post Found",
                    "Post Fetched Successfuly",
                    thread
                )
            );
            
        }

        [HttpPatch("{id}/{vote}")]
        [ServiceFilter(typeof(ValidateIdAttribute))]

        // Documentation
        [EndpointName("VotePost")]
        [EndpointSummary("VotePost")]
        [EndpointDescription("Vote for a thread based on the providade ID and kind of vote.")]
        [ProducesResponseType(typeof(NoContentResponse), StatusCodes.Status204NoContent, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ContentTooLarge), StatusCodes.Status413RequestEntityTooLarge, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Vote( [FromRoute] string id, [FromRoute] bool vote ) {
            
            if (!await _threadService.VoteAsync(id, vote))
            {
                return NotFound(new NotFound("Post Not Found", $"Post Not Found by ID: {id}."));
            }

            return StatusCode(
                204,
                noContentResponse
            );
             
        }

        [HttpPost]    
        [ServiceFilter(typeof(ValidateCaptcha))]
        
        // Documentation
        [Consumes("application/json")]  
        [EndpointName("CreatePost")]
        [EndpointSummary("CreatePost")]
        [EndpointDescription("Create a thread based on the body formed.")]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        
        public async Task<ActionResult> Create( [FromBody] ThreadDto threadDto, [FromHeader(Name = "X-CaptchaCode")] string CaptchaCode) {
            
            bool exists = await CheckMediaExists.CheckImageExistsAsync(threadDto.Media, threadDto.BoardType);
            
            if(!exists) return NotFound(new NotFound("Not Found","The media provided does not exist or was not found."));

            bool created = await _threadService.CreateAsync( threadDto );

            return created
                ? StatusCode(201, createdResponse)
                : StatusCode(500, internalError);

            
        }

    }
}
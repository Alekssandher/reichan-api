using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using reichan_api.src.QueryParams;
using reichan_api.src.Interfaces;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.DTOs.Responses;
using reichan_api.Filters;
using reichan_api.src.Utils;
using reichan_api.filters.Posts;

namespace reichan_api.src.Modules.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;

        // Responses objects
        private readonly InternalError internalError = new();
        private readonly NotFound postNotFound = new("Posts Not Found", "There are no posts mathing the query.");
        private readonly NoContentResponse noContentResponse = new();
        private readonly CreatedResponse createdResponse = new();

        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
            
        }

        
        [HttpGet]
        [ServiceFilter(typeof(ValidateQueryAttribute))]

        // Documentation
        [EndpointName("GetPosts")]
        [EndpointSummary("GetPosts")]
        [EndpointDescription("Retrieves a list of posts based on the provided query parameters.")]
        [ProducesResponseType(typeof(OkResponse<IReadOnlyList<PostResponseDTO>>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        
        public async Task<ActionResult> GetPosts([FromQuery] PostQueryParams queryParams)
        {    
            
            IReadOnlyList<PostResponseDTO> posts = await _postService.GetAllAsync(queryParams);
            
            if (!posts.Any()) return NotFound( postNotFound );
            
            return Ok(new OkResponse<IReadOnlyList<PostResponseDTO>>("Posts Found.", "Posts fetched successfuly.", posts));

            
            
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdAttribute))]
        // Documentation
        [EndpointName("GetPostById")]
        [EndpointSummary("GetPostById")]
        [EndpointDescription("Retrieves a post based on the provided ID.")]
        [ProducesResponseType(typeof(OkResponse<PostResponseDTO>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ContentTooLarge), StatusCodes.Status413RequestEntityTooLarge, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        
        public async Task<ActionResult> GetPostById( [FromRoute] string id ) {

            PostResponseDTO? post = await _postService.GetByIdAsync(id);

            if (post == null) return NotFound(new NotFound("Post Not Found", $"Post with ID '{id}' was not found."));
             
            return Ok(
                new OkResponse<PostResponseDTO>
                (
                    "Post Found",
                    "Post Fetched Successfuly",
                    post
                )
            );
            
        }

        [HttpPatch("{id}/{vote}")]
        [ServiceFilter(typeof(ValidateIdAttribute))]

        // Documentation
        [EndpointName("VotePost")]
        [EndpointSummary("VotePost")]
        [EndpointDescription("Vote for a post based on the providade ID and kind of vote.")]
        [ProducesResponseType(typeof(NoContentResponse), StatusCodes.Status204NoContent, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ContentTooLarge), StatusCodes.Status413RequestEntityTooLarge, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Vote( [FromRoute] string id, [FromRoute] bool vote ) {
            
            if (!await _postService.VoteAsync(id, vote))
            {
                return NotFound(new NotFound("Post Not Found", $"Post Not Found by ID: {id}."));
            }

            return StatusCode(
                204,
                noContentResponse
            );
             
        }

        [HttpPost]    
        [Consumes("application/json")]  
        [EndpointName("CreatePost")]

        // Documentation
        [EndpointSummary("CreatePost")]
        [EndpointDescription("Create a post based on the body formed.")]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Create( [FromBody] PostDto postDto ) {
            
            bool exists = await CheckMediaExists.CheckImageExistsAsync(postDto);
            
            if(!exists) return NotFound(new NotFound("Not Found","The media provided does not exist or was not found."));

            bool created = await _postService.CreateAsync( postDto );

            return created
                ? StatusCode(201, createdResponse)
                : StatusCode(500, internalError);

            
        }

    }
}
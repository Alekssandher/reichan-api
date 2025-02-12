using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;
using reichan_api.src.Interfaces;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.DTOs.Global;
using reichan_api.Filters;

namespace reichan_api.src.Modules.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;

        private readonly InternalError internalError = new();
        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        
        [HttpGet]
        [EndpointName("GetPosts")]
        [EndpointSummary("GetPosts")]
        [EndpointDescription("Retrieves a list of posts based on the provided query parameters.")]
        [ProducesResponseType(typeof(IReadOnlyList<PostResponseDTO>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        public async Task<ActionResult> GetPosts([FromQuery] PostQueryParams queryParams)
        {    
            FilterDefinition<PostModel> filter = queryParams.GetFilter();
            FindOptions<PostModel> options = queryParams.GetFindOptions();

            IReadOnlyList<PostResponseDTO> posts = await _postService.GetAllAsync(filter, options);
            
            if (!posts.Any()) return NotFound(new NotFound("Posts Not Found", "There are no posts mathing the query.") );
                
            
            return Ok(new ApiResponse<IReadOnlyList<PostResponseDTO>> { 
                Status = StatusCodes.Status200OK, 
                Data = posts 
            });
            
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdAttribute))]

        // Documentation
        [EndpointName("GetPostById")]
        [EndpointSummary("GetPostById")]
        [EndpointDescription("Retrieves a post based on the provided ID.")]
        [ProducesResponseType(typeof(PostResponseDTO), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]

        public async Task<ActionResult> GetPostById( [FromRoute] string id ) {

            PostResponseDTO? post = await _postService.GetByIdAsync(id);

            if (post == null) return NotFound(new NotFound("Post Not Found", $"Post with ID '{id}' was not found."));
             
            return Ok( new ApiResponse<PostResponseDTO> { 
                Status = StatusCodes.Status200OK, 
                Data = post
            });
            
        }

        [HttpPatch("{id}/{vote}")]

        [ServiceFilter(typeof(ValidateIdAttribute))]

        // Documentation
        [EndpointName("VotePost")]
        [EndpointSummary("VotePost")]
        [EndpointDescription("Vote for a post based on the providade ID and kind of vote.")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent, "application/json")]
        [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Vote( [FromRoute] string id, [FromRoute] bool vote ) {

            if (!await _postService.VoteAsync(id, vote))
            {
                return NotFound(new NotFound("Post Not Found", $"Post Not Found by ID: {id}."));
            }

            return NoContent();
             
        }

        [HttpPost]      
        [EndpointName("CreatePost")]
        [EndpointSummary("CreatePost")]
        [EndpointDescription("Create a post based on the body formed.")]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(InternalError), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Create( [FromBody] PostDto PostDto ) {
            
            bool created = await _postService.CreateAsync( PostDto );

            return created
                ? Created()
                : StatusCode(500, internalError);
            
        }

    }
}
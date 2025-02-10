using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;
using reichan_api.src.Interfaces;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.DTOs.Global;

namespace reichan_api.src.Modules.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        
        [HttpGet]
        [EndpointDescription("Retrieves a list of posts based on the provided query parameters.")]
        [ProducesResponseType(typeof(IReadOnlyList<PostResponseDTO>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]

        public async Task<ActionResult> GetPosts([FromQuery] PostQueryParams queryParams)
        {

            try
            {
                FilterDefinition<PostModel> filter = queryParams.GetFilter();
                FindOptions<PostModel> options = queryParams.GetFindOptions();

                IReadOnlyList<PostResponseDTO> posts = await _postService.GetAllAsync(filter, options);

                if (!posts.Any()) return NotFound(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Posts Not Found",
                    Detail = "There are no posts matching the query.",
                    Instance = HttpContext.Request.Path
                });
                    
                
                return Ok(new ApiResponse<IReadOnlyList<PostResponseDTO>> { 
                    Status = StatusCodes.Status200OK, 
                    Data = posts 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching posts.");

                return StatusCode(500, new ProblemDetails {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal error",
                    Detail = "Something went wrong at our side.",
                    Instance = HttpContext.Request.Path
                });

            }
        }

        [HttpGet("{id}")]
        [EndpointDescription("Retrieves a post based on the provided ID.")]
        [ProducesResponseType(typeof(PostResponseDTO), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]

        public async Task<ActionResult> GetPostById( [FromRoute] string id ) {

            try
            {
                PostResponseDTO? post = await _postService.GetByIdAsync(id);
            
                if (post == null ) return NotFound(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Post Not Found",
                    Detail = $"Post with ID '{id}' was not found.",
                    Instance = HttpContext.Request.Path
                });

                return Ok( new ApiResponse<PostResponseDTO> { 
                    Status = StatusCodes.Status200OK, 
                    Data = post
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching post.");

                return StatusCode(500, new ProblemDetails {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal error",
                    Detail = "Something went wrong at our side.",
                    Instance = HttpContext.Request.Path
                });
            }
        }

        [HttpPatch("{id}/{vote}")]
        [EndpointDescription("Vote for a post based on the providade ID and kind of vote.")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Vote( [FromRoute] string id, [FromRoute] bool vote ) {
            try
            {
                bool voted = await _postService.VoteAsync(id, vote);

                if(!voted) {
                    return NotFound(new ProblemDetails {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Post Not Found",
                        Detail = $"Post Not Found by ID: '${id}'.",
                        Instance = HttpContext.Request.Path
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching posts.");

                return StatusCode(500, new ProblemDetails {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal error",
                    Detail = "Something went wrong at our side.",
                    Instance = HttpContext.Request.Path
                });
            }
        }

        [HttpPost]      
        [EndpointDescription("Create a post based on the body formed.")]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult> Create( [FromBody] PostDto postDto ) {
            try
            {
                bool created = await _postService.CreateAsync( postDto );

                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching posts.");

                return StatusCode(500, new ProblemDetails {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal error",
                    Detail = "Something went wrong at our side.",
                    Instance = HttpContext.Request.Path
                });
            } 
        }

    }
}
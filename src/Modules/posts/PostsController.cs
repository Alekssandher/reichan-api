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
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<PostModel>>>> GetPosts([FromQuery] PostQueryParams queryParams)
        {

            try
            {
                FilterDefinition<PostModel> filter = queryParams.GetFilter();
                FindOptions<PostModel> options = queryParams.GetFindOptions();

                IReadOnlyList<PostModel> posts = await _postService.GetAllAsync(filter, options);

                if (!posts.Any()) return NotFound(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Posts Not Found",
                    Detail = "There are no posts matching the query.",
                    Instance = HttpContext.Request.Path
                });
                    

                return Ok(new ApiResponse<IReadOnlyList<PostModel>> { 
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
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponse<PostResponseDTO>>> GetPostById( string id ) {

            try
            {
                PostResponseDTO? post = await _postService.GetByIdAsync(id);
            
                if (post == null ) return NotFound(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Post Not Found",
                    Detail = $"Post Not Found by ID: '${id}'.",
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
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponse<string>>> VotePost( string id, bool vote ) {
            try
            {
                bool voted = await _postService.VotePostAsync(id, vote);

                if(!voted) {
                    return NotFound(new ProblemDetails {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Post Not Found",
                        Detail = $"Post Not Found by ID: '${id}'.",
                        Instance = HttpContext.Request.Path
                    });
                }

                return Ok( new ApiResponse<string> { 
                    Status = StatusCodes.Status200OK, 
                    Data = "Voted"
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

    }
}
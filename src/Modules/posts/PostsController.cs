using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ReichanApi.DTOs;
using ReichanApi.Interfaces;
using ReichanApi.Mappers;
using ReichanApi.Models;
using ReichanApi.QueryParams;

namespace ReichanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PostsResponseDTO>> GetPosts([FromQuery] PostQueryParams queryParams)
        {

            try
            {
                FilterDefinition<PostModel> filter = queryParams.GetFilter();
                FindOptions<PostModel> options = queryParams.GetFindOptions();

                IReadOnlyList<PostModel> posts = await _postService.GetAllAsync(filter, options);

                if (!posts.Any()) return NotFound(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No Posts Found",
                    Detail = "There are no posts matching the query.",
                    Instance = HttpContext.Request.Path
                });
                    

                return Ok(new PostsResponseDTO { 
                    Status = 202, 
                    Data = posts 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

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
        public async Task<ActionResult<PostResponseWrapperDTO>> GetPostById( string id ) {

            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid ID Format",
                    Detail = $"The provided ID '{id}' is not a valid MongoDB ObjectId.",
                    Instance = HttpContext.Request.Path
                });
            }


            PostModel? post = await _postService.GetByIdAsync(id);
            
            if (post == null ) return NotFound(new { success = false, message = "Post not found" });

            PostResponseDTO postDto = post.ToDto();

            return Ok( new PostResponseWrapperDTO { 
                Status = 202, 
                Post = postDto
            });
        }

    }
}
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("findAll")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        
        return Ok(new {
            success = true,
            users
        });
    }

    [ValidateModel]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto body)
    {
        try
        {
            await _userService.CreateAsync(body);
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
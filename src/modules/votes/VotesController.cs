using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class VotesController : ControllerBase {
    private readonly IVotesService _votesService;

    public VotesController(IVotesService votesService) {
        _votesService = votesService;
    }

    [HttpPost("vote")]
    public async Task<IActionResult> Vote() {

        return Ok(new {succes = true, message = "Voted"});
    }
    
}
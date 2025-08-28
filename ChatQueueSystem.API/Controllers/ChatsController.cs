
namespace ChatQueueSystem.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatSession([FromBody] CreateChatRequest request)
    {
        var command = new CreateChatSessionCommand(request.UserId);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("{sessionId:guid}/poll")]
    public async Task<IActionResult> PollChatSession(Guid sessionId)
    {
        var query = new PollChatSessionQuery(sessionId);
        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("status")]
    public IActionResult GetSystemStatus()
    {
        return Ok(new
        {
            Status = "Running",
            Timestamp = DateTime.UtcNow,
            Version = "1.0"
        });
    }
}

public record CreateChatRequest(string UserId);
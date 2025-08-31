namespace ChatQueueSystem.API.Api;

public static class ChatsApi
{
    public record CreateChatRequest(string UserId);
    public static void MapChatsApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/chats", async (IMediator mediator, CreateChatRequest request) =>
        {
            var command = new CreateChatSessionCommand(request.UserId);
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapGet("/api/v1/chats/{sessionId:guid}/poll", async (IMediator mediator, Guid sessionId) =>
        {
            var query = new PollChatSessionQuery(sessionId);
            var result = await mediator.Send(query);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapGet("/api/v1/chats/status", () =>
            Results.Ok(new { Status = "Running", Timestamp = DateTime.UtcNow, Version = "1.0" })
        );
    }
}


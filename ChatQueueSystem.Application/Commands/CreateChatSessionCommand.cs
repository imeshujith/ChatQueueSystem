
namespace ChatQueueSystem.Application.Commands;

public record CreateChatSessionCommand(string UserId) : IRequest<Result<ChatSessionResponse>>;

public record ChatSessionResponse(
    Guid SessionId, 
    bool IsQueued, 
    string Message, 
    DateTime CreatedAt);
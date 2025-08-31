namespace ChatQueueSystem.Application.Queries;

public record PollChatSessionQuery(Guid SessionId) : IRequest<Result<PollResponse>>;

public record PollResponse(
    Guid SessionId,
    bool IsActive,
    string Status,
    Guid? AssignedAgentId,
    DateTime LastPollTime);
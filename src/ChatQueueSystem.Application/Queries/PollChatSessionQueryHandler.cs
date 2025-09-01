namespace ChatQueueSystem.Application.Queries;

public class PollChatSessionQueryHandler : IRequestHandler<PollChatSessionQuery, Result<PollResponse>>
{
    private readonly IChatSessionRepository _chatSessionRepository;

    public PollChatSessionQueryHandler(IChatSessionRepository chatSessionRepository)
    {
        _chatSessionRepository = chatSessionRepository;
    }

    public async Task<Result<PollResponse>> Handle(PollChatSessionQuery request, CancellationToken cancellationToken)
    {
        var chatSession = await _chatSessionRepository.GetByIdAsync(request.SessionId);

        if (chatSession == null)
        {
            return Result<PollResponse>.Failure("Chat session not found");
        }

        chatSession.LastPollTime = DateTime.UtcNow;
        chatSession.PollMissCount = 0;

        await _chatSessionRepository.UpdateAsync(chatSession);

        return Result<PollResponse>.Success(new PollResponse(
            chatSession.Id,
            chatSession.Status == ChatStatus.Active,
            chatSession.Status.ToString(),
            chatSession.AssignedAgentId,
            chatSession.LastPollTime.Value));
    }
}
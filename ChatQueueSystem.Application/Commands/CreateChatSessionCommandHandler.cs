
namespace ChatQueueSystem.Application.Commands;

public class CreateChatSessionCommandHandler : IRequestHandler<CreateChatSessionCommand, Result<ChatSessionResponse>>
{
    private readonly IChatSessionRepository _chatSessionRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IQueueService _queueService;

    public CreateChatSessionCommandHandler(
        IChatSessionRepository chatSessionRepository,
        ITeamRepository teamRepository,
        IQueueService queueService)
    {
        _chatSessionRepository = chatSessionRepository;
        _teamRepository = teamRepository;
        _queueService = queueService;
    }

    public async Task<Result<ChatSessionResponse>> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        var activeTeams = await _teamRepository.GetActiveTeamsAsync();
        var overflowTeam = await _teamRepository.GetOverflowTeamAsync();
        
        bool isOfficeHours = IsOfficeHours();
        int totalCapacity = activeTeams.Sum(team => team.TotalCapacity);
        int totalQueueLength = await _chatSessionRepository.GetQueueLengthAsync();

        if (totalQueueLength >= totalCapacity * 1.5)
        {
            if (isOfficeHours && overflowTeam != null)
            {
                int overflowQueueLength = await _chatSessionRepository.GetOverflowQueueLengthAsync();
                if (overflowQueueLength >= overflowTeam.MaxQueueLength)
                {
                    return Result<ChatSessionResponse>.Failure("Queue is full. Chat refused.");
                }
            }
            else
            {
                return Result<ChatSessionResponse>.Failure("Queue is full. Chat refused.");
            }
        }

        var chatSession = new ChatSession
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Status = ChatStatus.Queued,
            CreatedAt = DateTime.UtcNow
        };

        await _chatSessionRepository.AddAsync(chatSession);
        await _queueService.EnqueueChatAsync(chatSession.Id);

        return Result<ChatSessionResponse>.Success(new ChatSessionResponse(
            chatSession.Id,
            true,
            "Chat session queued successfully",
            chatSession.CreatedAt));
    }

    private static bool IsOfficeHours()
    {
        TimeSpan now = DateTime.Now.TimeOfDay;
        return now >= new TimeSpan(9, 0, 0) && now <= new TimeSpan(17, 0, 0);
    }
}
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

        Console.WriteLine($"Total Capacity: {totalCapacity}");
        Console.WriteLine($"Total Queue Length: {totalQueueLength}");
        Console.WriteLine($"Is Office Hours: {isOfficeHours}");

        if (overflowTeam != null)
        {
            Console.WriteLine($"Overflow Team Max Queue Length: {overflowTeam.MaxQueueLength}");
        }

        int maxQueueLength = (int)(totalCapacity * 1.5);

        if (totalQueueLength >= maxQueueLength)
        {
            if (isOfficeHours && overflowTeam != null)
            {
                int overflowQueueLength = await _chatSessionRepository.GetOverflowQueueLengthAsync();
                Console.WriteLine($"Overflow Queue Length: {overflowQueueLength}");

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
        TimeSpan startOfficeHours = new TimeSpan(9, 0, 0); // 9:00 AM

        string? endOfficeHoursEnv = Environment.GetEnvironmentVariable("OFFICE_END_HOUR");
        TimeSpan endOfficeHours = TimeSpan.TryParse(endOfficeHoursEnv, out var parsedEndHour)
            ? parsedEndHour
            : new TimeSpan(17, 0, 0); // Default to 5:00 PM

        return now >= startOfficeHours && now <= endOfficeHours;
    }
}
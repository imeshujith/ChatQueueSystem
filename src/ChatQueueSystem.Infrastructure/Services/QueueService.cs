namespace ChatQueueSystem.Infrastructure.Services;

public class QueueService : IQueueService
{
    private readonly ConcurrentQueue<Guid> _chatQueue = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueueService> _logger;

    public QueueService(IServiceProvider serviceProvider, ILogger<QueueService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task EnqueueChatAsync(Guid chatSessionId)
    {
        _chatQueue.Enqueue(chatSessionId);
        _logger.LogInformation("Chat session {ChatSessionId} enqueued", chatSessionId);
        return Task.CompletedTask;
    }

    public async Task ProcessQueueAsync()
    {
        while (true)
        {
            if (_chatQueue.TryDequeue(out Guid chatSessionId))
            {
                _logger.LogInformation("Dequeued chat session {ChatSessionId} for assignment", chatSessionId);
                using var scope = _serviceProvider.CreateScope();
                var assignmentService = scope.ServiceProvider.GetRequiredService<IChatAssignmentService>();
                await assignmentService.AssignChatToAgentAsync(chatSessionId);
            }
            else
            {
                _logger.LogDebug("No chat sessions in queue to process");
            }
            await Task.Delay(1000);
        }
    }
}
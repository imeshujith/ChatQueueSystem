using System.Collections.Concurrent;
using ChatQueueSystem.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    public async Task EnqueueChatAsync(Guid chatSessionId)
    {
        _chatQueue.Enqueue(chatSessionId);
        _logger.LogInformation("Chat session {ChatSessionId} enqueued", chatSessionId);
    }

    public async Task ProcessQueueAsync()
    {
        while (true)
        {
            if (_chatQueue.TryDequeue(out Guid chatSessionId))
            {
                using var scope = _serviceProvider.CreateScope();
                var assignmentService = scope.ServiceProvider.GetRequiredService<IChatAssignmentService>();
                await assignmentService.AssignChatToAgentAsync(chatSessionId);
            }
            await Task.Delay(1000);
        }
    }
}
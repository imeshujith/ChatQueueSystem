namespace ChatQueueSystem.Domain.Interfaces;

public interface IQueueService
{
    Task EnqueueChatAsync(Guid chatSessionId);
    Task ProcessQueueAsync();
}
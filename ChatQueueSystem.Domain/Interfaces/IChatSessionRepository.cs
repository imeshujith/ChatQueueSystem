
namespace ChatQueueSystem.Domain.Interfaces;

public interface IChatSessionRepository
{
    Task AddAsync(ChatSession chatSession);
    Task<ChatSession?> GetByIdAsync(Guid id);
    Task UpdateAsync(ChatSession chatSession);
    Task<int> GetQueueLengthAsync();
    Task<int> GetOverflowQueueLengthAsync();
    Task<List<ChatSession>> GetInactiveSessionsAsync();
}
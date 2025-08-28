namespace ChatQueueSystem.Domain.Interfaces;

public interface IChatAssignmentService
{
    Task AssignChatToAgentAsync(Guid chatSessionId);
}
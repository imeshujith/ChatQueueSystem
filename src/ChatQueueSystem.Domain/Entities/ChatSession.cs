namespace ChatQueueSystem.Domain.Entities;

public class ChatSession
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AssignedAt { get; set; }
    public DateTime? LastPollTime { get; set; }
    public ChatStatus Status { get; set; } = ChatStatus.Queued;
    public Guid? AssignedAgentId { get; set; }
    public Agent? AssignedAgent { get; set; }
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    public int PollMissCount { get; set; }
}

public enum ChatStatus
{
    Queued = 1,
    Active = 2,
    Completed = 3,
    Refused = 4,
    Inactive = 5
}
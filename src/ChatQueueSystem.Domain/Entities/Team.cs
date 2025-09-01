namespace ChatQueueSystem.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Agent> Agents { get; set; } = new();
    public bool IsOverflow { get; set; }
    public TimeSpan ShiftStart { get; set; }
    public TimeSpan ShiftEnd { get; set; }
    public bool IsActive => DateTime.Now.TimeOfDay >= ShiftStart && DateTime.Now.TimeOfDay <= ShiftEnd;

    public int TotalCapacity => Agents.Sum(agent => agent.Capacity);
    public int MaxQueueLength => (int)Math.Floor(TotalCapacity * 1.5m);
}
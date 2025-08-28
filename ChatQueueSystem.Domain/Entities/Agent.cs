namespace ChatQueueSystem.Domain.Entities;

public class Agent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SeniorityLevel Seniority { get; set; }
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;
    public int CurrentChats { get; set; }
    public int MaxConcurrency => 10;
    public decimal EfficiencyMultiplier => Seniority switch
    {
        SeniorityLevel.Junior => 0.4m,
        SeniorityLevel.MidLevel => 0.6m,
        SeniorityLevel.Senior => 0.8m,
        SeniorityLevel.TeamLead => 0.5m,
        _ => 0.4m
    };
    public int Capacity => (int)Math.Floor(MaxConcurrency * EfficiencyMultiplier);
}

public enum SeniorityLevel
{
    Junior = 1,
    MidLevel = 2,
    Senior = 3,
    TeamLead = 4
}
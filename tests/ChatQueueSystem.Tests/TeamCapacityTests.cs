namespace ChatQueueSystem.Tests;

public class TeamCapacityTests
{
    [Fact]
    public void Team_TotalCapacity_Is_Sum_Of_Agent_Capacities()
    {
        var team = new Team
        {
            Agents = new List<Agent>
            {
                new Agent { Seniority = SeniorityLevel.Junior }, // 4
                new Agent { Seniority = SeniorityLevel.MidLevel }, // 6
                new Agent { Seniority = SeniorityLevel.Senior } // 8
            }
        };
        Assert.Equal(18, team.TotalCapacity);
    }

    [Fact]
    public void Team_MaxQueueLength_Is_OnePointFive_Times_Capacity_RoundedDown()
    {
        var team = new Team
        {
            Agents = new List<Agent>
            {
                new Agent { Seniority = SeniorityLevel.MidLevel }, // 6
                new Agent { Seniority = SeniorityLevel.MidLevel } // 6
            }
        };
        // Capacity = 12, MaxQueueLength = floor(12 * 1.5) = 18
        Assert.Equal(18, team.MaxQueueLength);
    }
}

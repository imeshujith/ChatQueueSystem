
namespace ChatQueueSystem.Tests;

public class OverflowTeamTests
{
    [Fact]
    public void OverflowTeam_IsOverflow_True()
    {
        var team = new Team { IsOverflow = true };
        Assert.True(team.IsOverflow);
    }

    [Fact]
    public void OverflowTeam_Agents_Are_Junior_By_Default()
    {
        var team = new Team { IsOverflow = true };
        team.Agents.Add(new Agent { Seniority = SeniorityLevel.Junior });
        foreach (var agent in team.Agents)
        {
            Assert.Equal(SeniorityLevel.Junior, agent.Seniority);
        }
    }
}


namespace ChatQueueSystem.Tests;

public class AgentAssignmentTests
{
    [Fact]
    public void Agent_Capacity_Is_Correct_Based_On_Seniority()
    {
        var junior = new Agent { Seniority = SeniorityLevel.Junior };
        var mid = new Agent { Seniority = SeniorityLevel.MidLevel };
        var senior = new Agent { Seniority = SeniorityLevel.Senior };
        var lead = new Agent { Seniority = SeniorityLevel.TeamLead };

        Assert.Equal(4, junior.Capacity); // 10 * 0.4
        Assert.Equal(6, mid.Capacity);    // 10 * 0.6
        Assert.Equal(8, senior.Capacity); // 10 * 0.8
        Assert.Equal(5, lead.Capacity);   // 10 * 0.5
    }
}

namespace ChatQueueSystem.Tests;

public class AgentShiftTests
{
    [Fact]
    public void Agent_IsActive_True_Within_Shift()
    {
        var now = DateTime.Now.TimeOfDay;
        var team = new Team { ShiftStart = now.Add(TimeSpan.FromHours(-1)), ShiftEnd = now.Add(TimeSpan.FromHours(1)) };
        Assert.True(team.IsActive);
    }

    [Fact]
    public void Agent_IsActive_False_Outside_Shift()
    {
        var now = DateTime.Now.TimeOfDay;
        var team = new Team { ShiftStart = now.Add(TimeSpan.FromHours(-3)), ShiftEnd = now.Add(TimeSpan.FromHours(-2)) };
        Assert.False(team.IsActive);
    }
}

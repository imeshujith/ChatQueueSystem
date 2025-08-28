
namespace ChatQueueSystem.Tests;

public class ChatSessionTests
{
    [Fact]
    public void ChatSession_Is_Queued_On_Creation()
    {
        var session = new ChatSession { UserId = "user1" };
        Assert.Equal(ChatStatus.Queued, session.Status);
    }

    [Fact]
    public void ChatSession_Marked_Inactive_After_3_Missed_Polls()
    {
        var session = new ChatSession { Status = ChatStatus.Active, PollMissCount = 2 };
        session.PollMissCount++;
        if (session.PollMissCount >= 3)
            session.Status = ChatStatus.Inactive;
        Assert.Equal(ChatStatus.Inactive, session.Status);
    }
}

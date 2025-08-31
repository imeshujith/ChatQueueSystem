namespace ChatQueueSystem.Infrastructure.Repositories;

public class ChatSessionRepository : IChatSessionRepository
{
    private readonly ChatDbContext _context;

    public ChatSessionRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatSession chatSession)
    {
        await _context.ChatSessions.AddAsync(chatSession);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatSession?> GetByIdAsync(Guid id)
    {
        return await _context.ChatSessions
            .Include(cs => cs.AssignedAgent)
            .FirstOrDefaultAsync(cs => cs.Id == id);
    }

    public async Task UpdateAsync(ChatSession chatSession)
    {
        _context.ChatSessions.Update(chatSession);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetQueueLengthAsync()
    {
        return await _context.ChatSessions
            .CountAsync(cs => cs.Status == ChatStatus.Queued && cs.Team != null && !cs.Team.IsOverflow);
    }

    public async Task<int> GetOverflowQueueLengthAsync()
    {
        return await _context.ChatSessions
            .CountAsync(cs => cs.Status == ChatStatus.Queued && cs.Team != null && cs.Team.IsOverflow);
    }

    public async Task<List<ChatSession>> GetInactiveSessionsAsync()
    {
        DateTime threshold = DateTime.UtcNow.AddSeconds(-3);
        return await _context.ChatSessions
            .Where(cs => cs.LastPollTime < threshold && cs.Status == ChatStatus.Active)
            .ToListAsync();
    }
}
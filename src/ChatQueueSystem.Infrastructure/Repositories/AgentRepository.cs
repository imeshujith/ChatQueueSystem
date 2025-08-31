namespace ChatQueueSystem.Infrastructure.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly ChatDbContext _context;

    public AgentRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<List<Agent>> GetByTeamIdAsync(Guid teamId)
    {
        return await _context.Agents
            .Where(a => a.TeamId == teamId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Agent agent)
    {
        _context.Agents.Update(agent);
        await _context.SaveChangesAsync();
    }
}
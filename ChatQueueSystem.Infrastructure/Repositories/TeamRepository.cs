using ChatQueueSystem.Domain.Entities;
using ChatQueueSystem.Domain.Interfaces;
using ChatQueueSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatQueueSystem.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ChatDbContext _context;

    public TeamRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetActiveTeamsAsync()
    {
        var teams = await _context.Teams
            .Include(t => t.Agents)
            .Where(t => !t.IsOverflow)
            .ToListAsync();

        return teams.Where(t => t.IsActive).ToList();
    }

    public async Task<Team?> GetOverflowTeamAsync()
    {
        return await _context.Teams
            .Include(t => t.Agents)
            .FirstOrDefaultAsync(t => t.IsOverflow);
    }
}
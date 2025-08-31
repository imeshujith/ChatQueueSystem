
namespace ChatQueueSystem.Domain.Interfaces;

public interface IAgentRepository
{
    Task<List<Agent>> GetByTeamIdAsync(Guid teamId);
    Task UpdateAsync(Agent agent);
}

namespace ChatQueueSystem.Domain.Interfaces;

public interface ITeamRepository
{
    Task<List<Team>> GetActiveTeamsAsync();
    Task<Team?> GetOverflowTeamAsync();
}
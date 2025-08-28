using ChatQueueSystem.Domain.Entities;
using ChatQueueSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChatQueueSystem.Infrastructure.Services;

public class ChatAssignmentService : IChatAssignmentService
{
    private readonly IChatSessionRepository _chatSessionRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly ILogger<ChatAssignmentService> _logger;

    public ChatAssignmentService(
        IChatSessionRepository chatSessionRepository,
        ITeamRepository teamRepository,
        IAgentRepository agentRepository,
        ILogger<ChatAssignmentService> logger)
    {
        _chatSessionRepository = chatSessionRepository;
        _teamRepository = teamRepository;
        _agentRepository = agentRepository;
        _logger = logger;
    }

    public async Task AssignChatToAgentAsync(Guid chatSessionId)
    {
        var chatSession = await _chatSessionRepository.GetByIdAsync(chatSessionId);
        if (chatSession == null)
        {
            _logger.LogWarning("Chat session {ChatSessionId} not found for assignment", chatSessionId);
            return;
        }

        var activeTeams = await _teamRepository.GetActiveTeamsAsync();
        var availableAgent = await FindAvailableAgentAsync(activeTeams);

        if (availableAgent == null && IsOfficeHours())
        {
            var overflowTeam = await _teamRepository.GetOverflowTeamAsync();
            if (overflowTeam != null)
            {
                availableAgent = await FindAvailableAgentAsync(new List<Team> { overflowTeam });
            }
        }

        if (availableAgent != null)
        {
            chatSession.AssignedAgentId = availableAgent.Id;
            chatSession.TeamId = availableAgent.TeamId;
            chatSession.Status = ChatStatus.Active;
            chatSession.AssignedAt = DateTime.UtcNow;

            availableAgent.CurrentChats++;
            if (availableAgent.CurrentChats >= availableAgent.Capacity)
            {
                availableAgent.IsAvailable = false;
            }

            await _chatSessionRepository.UpdateAsync(chatSession);
            await _agentRepository.UpdateAsync(availableAgent);

            _logger.LogInformation("Chat session {ChatSessionId} assigned to agent {AgentId}", chatSessionId, availableAgent.Id);
        }
        else
        {
            _logger.LogWarning("No available agent found for chat session {ChatSessionId}", chatSessionId);
        }
    }

    private async Task<Agent?> FindAvailableAgentAsync(List<Team> teams)
    {
        var allAgents = new List<Agent>();
        foreach (var team in teams)
        {
            var teamAgents = await _agentRepository.GetByTeamIdAsync(team.Id);
            allAgents.AddRange(teamAgents);
        }

        // Only assign to agents whose shift is active or who still have active chats to finish
        var eligibleAgents = allAgents
            .Where(agent =>
                (agent.Team.IsActive || agent.CurrentChats > 0) &&
                agent.IsAvailable &&
                agent.CurrentChats < agent.Capacity)
            .ToList();

        // Group by seniority, prefer lower first (junior, mid, senior, lead)
        foreach (var seniority in new[] { SeniorityLevel.Junior, SeniorityLevel.MidLevel, SeniorityLevel.Senior, SeniorityLevel.TeamLead })
        {
            var group = eligibleAgents.Where(a => a.Seniority == seniority).ToList();
            if (group.Count > 0)
            {
                // Round-robin: pick the agent in this group with the fewest current chats
                return group.OrderBy(a => a.CurrentChats).ThenBy(a => a.Id).FirstOrDefault();
            }
        }
        return null;
    }

    private static bool IsOfficeHours()
    {
        TimeSpan now = DateTime.Now.TimeOfDay;
        return now >= new TimeSpan(9, 0, 0) && now <= new TimeSpan(17, 0, 0);
    }
}
using ChatQueueSystem.Domain.Entities;
using ChatQueueSystem.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatQueueSystem.Infrastructure.Services;

public class SessionMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SessionMonitorService> _logger;

    public SessionMonitorService(IServiceProvider serviceProvider, ILogger<SessionMonitorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Session Monitor Service started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var chatSessionRepository = scope.ServiceProvider.GetRequiredService<IChatSessionRepository>();
                
                var inactiveSessions = await chatSessionRepository.GetInactiveSessionsAsync();
                foreach (var session in inactiveSessions)
                {
                    session.PollMissCount++;
                    if (session.PollMissCount >= 3)
                    {
                        session.Status = ChatStatus.Inactive;
                        _logger.LogWarning("Marked chat session {SessionId} as inactive after 3 missed polls", session.Id);
                    }
                    await chatSessionRepository.UpdateAsync(session);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring sessions");
            }
            
            await Task.Delay(3000, stoppingToken);
        }
    }
}
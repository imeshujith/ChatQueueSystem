using ChatQueueSystem.Domain.Interfaces;
using ChatQueueSystem.Infrastructure.Data;
using ChatQueueSystem.Infrastructure.Repositories;
using ChatQueueSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ChatQueueSystem.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("ChatQueueSystem.Infrastructure")
            ));

        services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();

        services.AddSingleton<IQueueService, QueueService>();
        services.AddScoped<IChatAssignmentService, ChatAssignmentService>();

        services.AddHostedService<QueueProcessorService>();
        services.AddHostedService<SessionMonitorService>();

        return services;
    }
}
namespace ChatQueueSystem.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("ChatQueueSystem.Infrastructure")
            ).EnableSensitiveDataLogging(false) // Disable EF Core sensitive data logging
             .LogTo(Console.WriteLine, LogLevel.None) // Disable EF Core logs
        );

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
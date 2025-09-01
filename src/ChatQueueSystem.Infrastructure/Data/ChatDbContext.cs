namespace ChatQueueSystem.Infrastructure.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    public DbSet<ChatSession> ChatSessions { get; set; } = null!;
    public DbSet<Agent> Agents { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.Agents)
                  .HasForeignKey(e => e.TeamId);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Agents)
                  .WithOne(a => a.Team)
                  .HasForeignKey(a => a.TeamId);
        });

        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AssignedAgent)
                  .WithMany()
                  .HasForeignKey(e => e.AssignedAgentId);
            entity.HasOne(e => e.Team)
                  .WithMany()
                  .HasForeignKey(e => e.TeamId);
        });
    }
}
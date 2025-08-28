
namespace ChatQueueSystem.API;

public static class DatabaseSeeder
{
    public static void Seed(ChatDbContext context)
    {
        context.Database.Migrate();

        if (!context.Teams.Any())
        {
            // Team A: 1x team lead, 2x mid-level, 1x junior
            var teamA = new Team
            {
                Name = "Team A",
                ShiftStart = new TimeSpan(9, 0, 0),
                ShiftEnd = new TimeSpan(17, 0, 0),
                IsOverflow = false,
                Agents = new List<Agent>
                {
                    new Agent { Name = "A-TeamLead", Seniority = SeniorityLevel.TeamLead },
                    new Agent { Name = "A-Mid1", Seniority = SeniorityLevel.MidLevel },
                    new Agent { Name = "A-Mid2", Seniority = SeniorityLevel.MidLevel },
                    new Agent { Name = "A-Junior", Seniority = SeniorityLevel.Junior }
                }
            };
            // Team B: 1x senior, 1x mid-level, 2x junior
            var teamB = new Team
            {
                Name = "Team B",
                ShiftStart = new TimeSpan(9, 0, 0),
                ShiftEnd = new TimeSpan(17, 0, 0),
                IsOverflow = false,
                Agents = new List<Agent>
                {
                    new Agent { Name = "B-Senior", Seniority = SeniorityLevel.Senior },
                    new Agent { Name = "B-Mid", Seniority = SeniorityLevel.MidLevel },
                    new Agent { Name = "B-Junior1", Seniority = SeniorityLevel.Junior },
                    new Agent { Name = "B-Junior2", Seniority = SeniorityLevel.Junior }
                }
            };
            // Team C: 2x mid-level (night shift team)
            var teamC = new Team
            {
                Name = "Team C",
                ShiftStart = new TimeSpan(0, 0, 0),
                ShiftEnd = new TimeSpan(8, 0, 0),
                IsOverflow = false,
                Agents = new List<Agent>
                {
                    new Agent { Name = "C-Mid1", Seniority = SeniorityLevel.MidLevel },
                    new Agent { Name = "C-Mid2", Seniority = SeniorityLevel.MidLevel }
                }
            };
            // Overflow team: 6x junior
            var overflowTeam = new Team
            {
                Name = "Overflow",
                ShiftStart = new TimeSpan(9, 0, 0),
                ShiftEnd = new TimeSpan(17, 0, 0),
                IsOverflow = true,
                Agents = Enumerable.Range(1, 6).Select(i => new Agent { Name = $"Overflow-Junior{i}", Seniority = SeniorityLevel.Junior }).ToList()
            };

            context.Teams.AddRange(teamA, teamB, teamC, overflowTeam);
            context.SaveChanges();
        }
    }
}

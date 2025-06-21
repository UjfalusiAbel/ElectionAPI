using Election.Core;
using Election.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Election.Api.Hubs;

namespace Election.Api.Services
{
    public class CandidateGeneratorService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<CandidateHub> _hubContext;
        private Timer? _timer;
        private bool _running = false;
        private static readonly string[] Names = { "Alex Pop", "Mihai Ionescu", "John Doe", "Jane Smith", "Maria Stan", "Elena Tudor", "David Black", "Sophia White", "Paul Brown", "Anna Blue" };
        private static readonly string[] Parties = { "Green Party", "Liberal Alliance", "Social Democrats", "Conservative Union", "Progressive Movement", "People's Party", "Liberty Party" };
        private static readonly string[] Descs = { "Community leader.", "Economist.", "Healthcare professional.", "Entrepreneur.", "Teacher.", "Engineer.", "Lawyer.", "Planner.", "Policymaker.", "Advocate for equality." };

        public CandidateGeneratorService(IServiceScopeFactory scopeFactory, IHubContext<CandidateHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        public void Start()
        {
            if (_running) return;
            _running = true;
            _timer = new Timer(GenerateCandidate, null, 0, 2000);
        }

        public void Stop()
        {
            _running = false;
            _timer?.Dispose();
        }

        private async void GenerateCandidate(object? state)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var rnd = new Random();
            var candidate = new Candidate
            {
                Id = Guid.NewGuid(),
                Name = Names[rnd.Next(Names.Length)],
                Party = Parties[rnd.Next(Parties.Length)],
                Description = Descs[rnd.Next(Descs.Length)],
                Image = $"https://randomuser.me/api/portraits/men/{rnd.Next(10, 99)}.jpg"
            };
            db.Candidates.Add(candidate);
            await db.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("CandidateGenerated", candidate);
        }
    }
}

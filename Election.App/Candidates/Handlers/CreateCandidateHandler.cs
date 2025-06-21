using Election.Core;
using Election.Core.Models;
using Election.App.Features.Candidates.Commands;
using MediatR;

namespace Election.App.Features.Candidates.Handlers
{
    public class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, Candidate>
    {
        private readonly ApplicationDbContext _context;
        public CreateCandidateHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Candidate> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = new Candidate
            {
                Name = request.Name,
                Party = request.Party,
                Description = request.Description,
                Image = request.Image
            };
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync(cancellationToken);
            return candidate;
        }
    }
}

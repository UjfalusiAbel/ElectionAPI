using Election.Core;
using Election.Core.Models;
using Election.App.Features.Candidates.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Election.App.Features.Candidates.Handlers
{
    public class UpdateCandidateHandler : IRequestHandler<UpdateCandidateCommand, Candidate?>
    {
        private readonly ApplicationDbContext _context;
        public UpdateCandidateHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Candidate?> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (candidate == null) return null;
            candidate.Name = request.Name;
            candidate.Party = request.Party;
            candidate.Description = request.Description;
            candidate.Image = request.Image;
            await _context.SaveChangesAsync(cancellationToken);
            return candidate;
        }
    }
}

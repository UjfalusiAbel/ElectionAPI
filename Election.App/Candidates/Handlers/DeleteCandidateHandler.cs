using Election.Core;
using Election.App.Features.Candidates.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Election.App.Features.Candidates.Handlers
{
    public class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public DeleteCandidateHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (candidate == null) return false;
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

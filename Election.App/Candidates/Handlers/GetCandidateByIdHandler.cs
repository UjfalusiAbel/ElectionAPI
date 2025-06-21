using Election.Core;
using Election.Core.Models;
using Election.App.Features.Candidates.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Election.App.Features.Candidates.Handlers
{
    public class GetCandidateByIdHandler : IRequestHandler<GetCandidateByIdQuery, Candidate?>
    {
        private readonly ApplicationDbContext _context;
        public GetCandidateByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Candidate?> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        }
    }
}

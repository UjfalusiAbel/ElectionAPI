using Election.Core;
using Election.Core.Models;
using Election.App.Features.Candidates.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Election.App.Features.Candidates.Handlers
{
    public class GetAllCandidatesHandler : IRequestHandler<GetAllCandidatesQuery, List<Candidate>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllCandidatesHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Candidate>> Handle(GetAllCandidatesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Candidates.ToListAsync(cancellationToken);
        }
    }
}

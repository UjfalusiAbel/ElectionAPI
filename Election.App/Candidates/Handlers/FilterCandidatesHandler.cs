using Election.Core;
using Election.Core.Models;
using Election.App.Features.Candidates.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Election.App.Features.Candidates.Handlers
{
    public class FilterCandidatesHandler : IRequestHandler<FilterCandidatesQuery, List<Candidate>>
    {
        private readonly ApplicationDbContext _context;
        public FilterCandidatesHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Candidate>> Handle(FilterCandidatesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Candidates.AsQueryable();
            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(c => c.Name.Contains(request.Name));
            if (!string.IsNullOrEmpty(request.Party))
                query = query.Where(c => c.Party.Contains(request.Party));
            return await query.ToListAsync(cancellationToken);
        }
    }
}

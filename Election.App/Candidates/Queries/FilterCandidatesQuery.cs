using Election.Core.Models;
using MediatR;

namespace Election.App.Features.Candidates.Queries
{
    public class FilterCandidatesQuery : IRequest<List<Candidate>>
    {
        public string? Name { get; set; }
        public string? Party { get; set; }
    }
}

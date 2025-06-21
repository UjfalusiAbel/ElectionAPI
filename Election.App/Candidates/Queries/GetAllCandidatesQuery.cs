using Election.Core.Models;
using MediatR;

namespace Election.App.Features.Candidates.Queries
{
    public class GetAllCandidatesQuery : IRequest<List<Candidate>>
    {
    }
}

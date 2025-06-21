using Election.Core.Models;
using MediatR;

namespace Election.App.Features.Candidates.Queries
{
    public class GetCandidateByIdQuery : IRequest<Candidate?>
    {
        public Guid Id { get; set; }
        public GetCandidateByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

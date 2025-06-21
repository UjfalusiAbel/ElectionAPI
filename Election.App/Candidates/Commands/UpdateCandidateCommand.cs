using Election.Core.Models;
using MediatR;

namespace Election.App.Features.Candidates.Commands
{
    public class UpdateCandidateCommand : IRequest<Candidate?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}

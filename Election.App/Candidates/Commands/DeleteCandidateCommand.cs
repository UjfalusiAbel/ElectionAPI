using MediatR;

namespace Election.App.Features.Candidates.Commands
{
    public class DeleteCandidateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteCandidateCommand(Guid id)
        {
            Id = id;
        }
    }
}

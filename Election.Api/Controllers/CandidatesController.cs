using Election.App.Features.Candidates.Commands;
using Election.App.Features.Candidates.Queries;
using Election.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Election.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CandidatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Candidate>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCandidatesQuery());
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Candidate>>> Filter([FromQuery] string? name, [FromQuery] string? party)
        {
            var result = await _mediator.Send(new FilterCandidatesQuery { Name = name, Party = party });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCandidateByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Candidate>> Create([FromBody] CreateCandidateCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Candidate>> Update(Guid id, [FromBody] UpdateCandidateCommand command)
        {
            if (id != command.Id) return BadRequest();
            var result = await _mediator.Send(command);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCandidateCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

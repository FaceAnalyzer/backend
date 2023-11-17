using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[Route("projects")]
public class ProjectController : ControllerBase
{
    private readonly ISender _mediator;

    public ProjectController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectCommand dto)
    {
        var project = await _mediator.Send(dto);
        return Created($"/projects/{project.Id}", project);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProjectDto>> Delete(int id)
    {
        var command = new DeleteProjectCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
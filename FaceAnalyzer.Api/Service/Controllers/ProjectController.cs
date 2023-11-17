using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[Route("projects")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class ProjectController : ControllerBase
{
    private readonly ISender _mediator;

    public ProjectController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        var command = new CreateProjectCommand(Name: dto.Name);
        var project = await _mediator.Send(command);
        return Created($"/projects/{project.Id}", project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> Edit(int id, [FromBody] EditProjectDto request)
    {
        var command = new EditProjectCommand(id, request.Name);
        var project = await _mediator.Send(command);
        return Ok(project);
    }
    

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProjectDto>> Delete(int id)
    {
        var command = new DeleteProjectCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/researcher/add")]
    public async Task<ActionResult> GrantPermission(int id, [FromBody]GrantProjectPermissionDto request)
    {
        var command = new GrantProjectPermissionCommand(id, request.ResearchersIds);
        var project = await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPut("{id}/researcher/remove")]
    public async Task<ActionResult> RevokePermission(int id, [FromBody]GrantProjectPermissionDto request)
    {
        var command = new RevokeProjectPermissionCommand(id, request.ResearchersIds);
        var project = await _mediator.Send(command);
        return NoContent();
    }

}
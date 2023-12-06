using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Controllers;

[Route("projects")]
public class ProjectController : ControllerBase
{
    private readonly ISender _mediator;

    public ProjectController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Researcher)}")]
    public async Task<ActionResult<QueryResult<ProjectDto>>> Get([FromQuery] ProjectQueryDto dto)
    {
        var query = new GetProjectsQuery(Id: null, Name: dto.ProjectName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Researcher)}")]
    public async Task<ActionResult<QueryResult<ProjectDto>>> Get(int id)
    {
        var query = new GetProjectsQuery(Id: id, Name: null);
        var result = await _mediator.Send(query);
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException(nameof(Project), id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        var command = new CreateProjectCommand(Name: dto.Name);
        var project = await _mediator.Send(command);
        return Created($"/projects/{project.Id}", project);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ProjectDto>> Edit(int id, [FromBody] EditProjectDto request)
    {
        var command = new EditProjectCommand(id, request.Name);
        var project = await _mediator.Send(command);
        return Ok(project);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ProjectDto>> Delete(int id)
    {
        var command = new DeleteProjectCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/researcher/add")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [SwaggerRequestExample(typeof(GrantRevokeProjectPermissionDto), typeof(GrantRevokeProjectPermissionDtoExample))]
    public async Task<ActionResult> GrantPermission(int id, [FromBody] GrantRevokeProjectPermissionDto request)
    {
        var command = new GrantProjectPermissionCommand(id, request.ResearchersIds);
        var project = await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/researcher/remove")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [SwaggerRequestExample(typeof(GrantRevokeProjectPermissionDto), typeof(GrantRevokeProjectPermissionDtoExample))]
    public async Task<ActionResult> RevokePermission(int id, [FromBody] GrantRevokeProjectPermissionDto request)
    {
        var command = new RevokeProjectPermissionCommand(id, request.ResearchersIds);
        var project = await _mediator.Send(command);
        return NoContent();
    }
}
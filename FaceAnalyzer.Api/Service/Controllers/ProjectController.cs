using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[Route("projects")]
public class ProjectController : ControllerBase
{
    private readonly ProjectBusinessModel _businessModel;

    public ProjectController(ProjectBusinessModel businessModel)
    {
        _businessModel = businessModel;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectDto dto)
    {
        var project = await _businessModel.Create(dto);
        return Created($"/projects/{project.Id}", project);
    }
}
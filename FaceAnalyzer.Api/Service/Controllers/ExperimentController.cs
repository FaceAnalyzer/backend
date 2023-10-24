using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
public class ExperimentController: ControllerBase
{
    private readonly ExperimentBusinessModel _businessModel;

    public ExperimentController(ExperimentBusinessModel businessModel)
    {
        _businessModel = businessModel;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ExperimentDto>> Create([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Create(dto);
        return Created($"/experiments/{result.Id}", result);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<ActionResult<ExperimentDto>> Edit([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Edit(dto);
        if (result is null) return BadRequest();
        return Ok();
    }

    [HttpDelete]
    [AllowAnonymous]
    public async Task<ActionResult<ExperimentDto>> Delete([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Delete(dto);
        if (result is null) return BadRequest();
        return NoContent();
    }
}
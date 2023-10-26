using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
public class ExperimentController : ControllerBase
{
    private readonly ExperimentBusinessModel _businessModel;

    public ExperimentController(ExperimentBusinessModel businessModel)
    {
        _businessModel = businessModel;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExperimentDto>>> Get()
    {
        var result = await _businessModel.Get();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExperimentDto>> Create([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Create(dto);
        if (result is null) return BadRequest();
        return Created($"/experiments/{result.Id}", result);
    }

    [HttpPut]
    public async Task<ActionResult<ExperimentDto>> Edit([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Edit(dto);
        if (result is null) return BadRequest();
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<ExperimentDto>> Delete([FromBody] ExperimentDto dto)
    {
        var result = await _businessModel.Delete(dto);
        if (result is null) return BadRequest();
        return NoContent();
    }
}
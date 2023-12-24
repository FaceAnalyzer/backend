using System.Globalization;
using System.IO.Compression;
using CsvHelper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
public class ExperimentController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ILogger<ExperimentController> _logger;

    public ExperimentController(ISender mediator, ILogger<ExperimentController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Retrieve a single experiment",
        "Retrieve a single experiment given its Id.",
        OperationId = $"{nameof(ExperimentController)}_get")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ExperimentDto))]
    public async Task<ActionResult<ExperimentDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetExperimentsQuery(id, null));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException(nameof(Experiment), id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpGet]
    [SwaggerOperation("Retrieve a list of experiments",
        "Retrieve a list of experiments filtered by their [projectId] (if [projectId] is empty all experiments are returned).",
        OperationId = $"{nameof(Experiment)}_get_list")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<ExperimentDto>))]
    public async Task<ActionResult<List<ExperimentDto>>> Get(int? projectId)
    {
        var result = await _mediator.Send(new GetExperimentsQuery(null, projectId));
        return Ok(result);
    }

    [HttpGet("{experimentId:int}/export")]
    [SwaggerOperation("Export the experiment in full",
        "Export a full experiment given its [experimentId] as a single compressed file including all its reactions (optionally you can specify a list of [reactionIds] to export a subset of reactions)")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(File))]
    public async Task<IActionResult> ExportExperiment(int experimentId,
        [FromQuery] [SwaggerParameter("A comma seperated list of stimuli ids to be excluded [\"1,2,3\"]")]
        string? stimuliIds)
    {
        var idsString = stimuliIds?.Trim().TrimStart(',').TrimEnd(',').Split(',');
        var stimuliIdsInt = idsString?.Select(int.Parse).ToList();
        var query = new ExportExperimentQuery(ExperimentId: experimentId, stimuliIdsInt);
        var experiment = await _mediator.Send(query);

        var tempZip = Path.GetTempPath() + $"{nameof(Experiment)}_{experimentId}.zip";
        await using (var zipFile = System.IO.File.Create(tempZip))
        using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Create))
        {
            foreach (var stimuli in experiment.Stimuli)
            {
                foreach (var reaction in stimuli.Reactions)
                {
                    var emotionsRecord = reaction.Emotions.GroupBy(e => e.TimeOffset).Select(g => g.ToList()).ToList();
                    var tempCsv = Path.GetTempPath() + $"{stimuli.Id}.{stimuli.Name}\\{nameof(Reaction)}_{reaction.ParticipantName.Replace(' ', '-')}.csv";
                    await using var csvFile = System.IO.File.Create(tempCsv);
                    await using var fileWriter = new StreamWriter(csvFile, leaveOpen: false);
                    await using (var csv = new CsvWriter(fileWriter, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<EmotionCsv>();
                        await csv.NextRecordAsync();
                        foreach (var records in emotionsRecord)
                        {
                            csv.WriteRecord(new EmotionCsv(records));
                            await csv.NextRecordAsync();
                        }
                    }

                    zipArchive.CreateEntryFromFile(tempCsv, Path.GetFileName(tempCsv));
                }
            }
        }

        var outputStream = new FileStream(tempZip, FileMode.Open);
        return new FileStreamResult(outputStream, "application/zip")
            { FileDownloadName = $"{nameof(Experiment)}_{experimentId}" };
    }

    [HttpPost]
    [SwaggerOperation("Create an experiment",
        "Create an experiment given its [Name]. The created experiment is associated with a project [projectId].",
        OperationId = $"{nameof(Experiment)}_create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ExperimentDto))]
    [SwaggerRequestExample(typeof(CreateExperimentDto), typeof(CreateExperimentDtoExample))]
    public async Task<ActionResult<ExperimentDto>> Create([FromBody] CreateExperimentDto dto)
    {
        var command = new CreateExperimentCommand(dto.Name, dto.Description, dto.ProjectId);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Modify an experiment",
        "Modify an Experiment (the only modifiable value is the [Name]).",
        OperationId = $"{nameof(Experiment)}_edit")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ExperimentDto))]
    public async Task<ActionResult<ExperimentDto>> Edit(int id, [FromBody] EditExperimentDto dto)
    {
        var command = new EditExperimentCommand(
            id,
            dto.Name,
            dto.Description,
            dto.ProjectId
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete an experiment",
        "Delete an experiment given its Id.",
        OperationId = $"{nameof(Experiment)}_delete")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<ExperimentDto>> Delete(int id)
    {
        var command = new DeleteExperimentCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
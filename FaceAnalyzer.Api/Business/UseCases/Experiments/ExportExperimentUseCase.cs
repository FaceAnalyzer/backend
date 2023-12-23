using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class ExportExperimentUseCase : BaseUseCase, IRequestHandler<ExportExperimentQuery, ExportExperimentDto>
{
    private readonly ILogger<ExportExperimentDto> _logger;

    public ExportExperimentUseCase(IMapper mapper, AppDbContext dbContext, ILogger<ExportExperimentDto> logger) : base(
        mapper, dbContext)
    {
        _logger = logger;
    }

    public async Task<ExportExperimentDto> Handle(ExportExperimentQuery request, CancellationToken cancellationToken)
    {
        var experiment = (await DbContext
                .Experiments
                .Include(e => e.Stimuli)
                .ProjectTo<ExportExperimentDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
            ).FirstOrDefault(e => e.Id == request.ExperimentId);

        if (experiment == null) throw new EntityNotFoundException(nameof(Experiment), request.ExperimentId);
        if (request.StimuliIds != null)
        {
            foreach (var stimuliId in request.StimuliIds)
            {
                var stimuli = experiment.Stimuli.FirstOrDefault(s => s.Id == stimuliId);
                if (stimuli != null) experiment.Stimuli.Remove(stimuli);
            }
        }
        return experiment;
    }
}
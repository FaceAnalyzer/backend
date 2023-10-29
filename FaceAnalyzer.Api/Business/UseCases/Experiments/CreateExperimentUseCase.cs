using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class CreateExperimentUseCase: BaseUseCase<ExperimentDto, ExperimentDto>
{
    public CreateExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<ExperimentDto> Execute(ExperimentDto input)
    {
        var project = DbContext.Find<Project>(input.ProjectId);
        if (project is null)
        {
            // TODO: Change to EntityNotFoundException
            throw new Exception();
        }

        var experiment = Mapper.Map<Experiment>(input);
        DbContext.Add(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
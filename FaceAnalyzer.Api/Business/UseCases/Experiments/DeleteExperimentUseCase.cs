using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class DeleteExperimentUseCase: BaseUseCase<ExperimentDto, ExperimentDto>
{
    public DeleteExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<ExperimentDto> Execute(ExperimentDto input)
    {
        var experiment = DbContext.Find<Experiment>(input.Id);
        if (experiment is null)
        {
            throw new Exception();
        }

        experiment.DeletedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
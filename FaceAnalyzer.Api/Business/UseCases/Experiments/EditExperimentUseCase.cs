using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class EditExperimentUseCase: BaseUseCase<ExperimentDto, ExperimentDto>
{
    public EditExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<ExperimentDto> Execute(ExperimentDto input)
    {
        var experiment = DbContext.Find<Experiment>(input.Id);
        if (experiment is null)
        {
            // TODO: Change to Entity Not Found
            throw new Exception();
        }

        experiment.Name = input.Name;
        experiment.Description = input.Description;
        experiment.UpdatedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
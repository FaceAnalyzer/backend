using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class DeleteExperimentUseCase: BaseUseCase, IRequestHandler<DeleteExperimentCommand, ExperimentDto>
{
    public DeleteExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task<ExperimentDto> Handle(DeleteExperimentCommand request, CancellationToken cancellationToken)
    {
        var experiment = DbContext.Find<Experiment>(request.Id);
        if (experiment is null)
        {
            throw new Exception();
        }

        experiment.DeletedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
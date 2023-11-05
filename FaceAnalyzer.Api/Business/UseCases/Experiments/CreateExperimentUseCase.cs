using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class CreateExperimentUseCase: BaseUseCase, IRequestHandler<CreateExperimentCommand, ExperimentDto>
{
    public CreateExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task<ExperimentDto> Handle(CreateExperimentCommand request, CancellationToken cancellationToken)
    {
        var project = DbContext.Find<Project>(request.ProjectId);
        if (project is null)
        {
            // TODO: Change to EntityNotFoundException
            throw new Exception();
        }

        var experiment = Mapper.Map<Experiment>(request);
        DbContext.Add(experiment);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
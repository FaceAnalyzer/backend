using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class EditExperimentUseCase: BaseUseCase, IRequestHandler<EditExperimentCommand, ExperimentDto>
{
    public EditExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ExperimentDto> Handle(EditExperimentCommand request, CancellationToken cancellationToken)
    {
        var experiment = DbContext.Find<Experiment>(request.Id);
        if (experiment is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no experiment with this id ({request.Id}) was found")
                .Build();
        }

        if (request.ProjectId is not null)
        {
            var project = DbContext.Find<Project>(request.ProjectId);
            if (project is null)
            {
                throw new InvalidArgumentsExceptionBuilder()
                    .AddArgument(nameof(request.ProjectId),
                        $"no project with this id ({request.ProjectId}) was found")
                    .Build();
            }
            experiment.ProjectId = project.Id;
        }

        experiment.Name = request.Name;
        experiment.Description = request.Description;
        experiment.UpdatedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ExperimentDto>(experiment);
    }
}
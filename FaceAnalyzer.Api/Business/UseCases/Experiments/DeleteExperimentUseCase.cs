using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class DeleteExperimentUseCase: BaseUseCase, IRequestHandler<DeleteExperimentCommand>
{
    public DeleteExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task Handle(DeleteExperimentCommand request, CancellationToken cancellationToken)
    {
        var experiment = DbContext.Find<Experiment>(request.Id);
        if (experiment is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no experiment with this id ({request.Id}) was found")
                .Build();
        }

        experiment.DeletedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
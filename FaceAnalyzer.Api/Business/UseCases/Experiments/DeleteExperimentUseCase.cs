using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class DeleteExperimentUseCase : BaseUseCase, IRequestHandler<DeleteExperimentCommand>
{
    public DeleteExperimentUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }


    public async Task Handle(DeleteExperimentCommand request, CancellationToken cancellationToken)
    {
        var experiment = await DbContext.Experiments
            .Include(r => r.Stimuli)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (experiment is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no experiment with this id ({request.Id}) was found")
                .Build();
        }

        foreach (var stimuli in experiment.Stimuli)
        {
            DbContext.Delete(stimuli);
        }
    
        DbContext.Delete(experiment);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
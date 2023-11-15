using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.StimuliUseCases;

public class DeleteStimuliUseCase : BaseUseCase, IRequestHandler<DeleteStimuliCommand>
{
    public async Task Handle(DeleteStimuliCommand request, CancellationToken cancellationToken)
    {
        var stimuli = await DbContext.Stimuli
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (stimuli is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no stimuli with this id ({request.Id}) was found")
                .Build();
        }
        
        DbContext.Delete(stimuli);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public DeleteStimuliUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}

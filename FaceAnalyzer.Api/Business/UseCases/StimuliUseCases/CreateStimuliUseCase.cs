using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.StimuliUseCases;

public class CreateStimuliUseCase : BaseUseCase, IRequestHandler<CreateStimuliCommand, StimuliDto>
{
    public CreateStimuliUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<StimuliDto> Handle(CreateStimuliCommand request, CancellationToken cancellationToken)
    {
        var experimentExists =
            await DbContext.Experiments
                .AnyAsync(e => e.Id == request.ExperimentId,
                            cancellationToken);
        if (!experimentExists)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.ExperimentId),
                    $"no experiment with this id ({request.ExperimentId}) was found")
                .Build();
            
        }
        var stimuli = new Stimuli
        {
            Description = request.Description,
            Link = request.Link,
            ExperimentId = request.ExperimentId,
        };
        DbContext.Stimuli.Add(stimuli);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<StimuliDto>(stimuli);
    }
}
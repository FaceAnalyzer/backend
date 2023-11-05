using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;


namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class CreateReactionUseCase : BaseUseCase, IRequestHandler<CreateReactionCommand, ReactionDto>
{
    public CreateReactionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
    {
        var stimuli = DbContext.Find<Stimuli>(request.StimuliId);
        if (stimuli is null)
        {
            // TODO: Change to EntityNotFoundException
            throw new Exception();
        }

        var reaction = new Reaction
        {
            ParticipantName = request.ParticipantName,
            StimuliId = request.StimuliId,
        };
        DbContext.Add(reaction);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ReactionDto>(reaction);
    }
}
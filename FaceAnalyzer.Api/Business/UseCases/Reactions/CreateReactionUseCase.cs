using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class CreateReactionUseCase : BaseUseCase, IRequestHandler<CreateReactionCommand, ReactionDto>
{
    public CreateReactionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ReactionDto> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
    {
        var stimuliExist = await DbContext.Stimuli
            .AnyAsync(stimuli => stimuli.Id == request.StimuliId, cancellationToken);
        if (!stimuliExist)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.StimuliId),
                    $"no stimuli with this id ({request.StimuliId}) was found")
                .Build();
        }


        var reaction = new Reaction
        {
            ParticipantName = request.ParticipantName,
            StimuliId = request.StimuliId,
        };

        reaction.Emotions = request.EmotionReadings
            .SelectMany(reading =>
            {
                var values = new List<Emotion>();
                foreach (var kv in reading.Values)
                {
                    var emotion = new Emotion
                    {
                        EmotionType = kv.Key,
                        Value = kv.Value,
                        TimeOffset = reading.Time
                    };
                    values.Add(emotion);
                }

                return values;
            }).ToList();
        DbContext.Add(reaction);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ReactionDto>(reaction);
    }
}
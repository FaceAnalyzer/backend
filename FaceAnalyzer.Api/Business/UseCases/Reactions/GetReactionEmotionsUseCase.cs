using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class GetReactionEmotionsUseCase : BaseUseCase,
    IRequestHandler<GetReactionEmotionsQuery, QueryResult<EmotionDto>>
{
    public GetReactionEmotionsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<QueryResult<EmotionDto>> Handle(GetReactionEmotionsQuery request,
        CancellationToken cancellationToken)
    {
        var reactionExist =
            await DbContext.Reactions.AnyAsync(reaction => reaction.Id == request.ReactionId, cancellationToken);
        if (!reactionExist)
        {
            throw new EntityNotFoundException(nameof(Reaction), request.ReactionId);
        }

        var emotions = await DbContext.Emotions
            .Where(emotion => emotion.ReactionId == request.ReactionId)
            .ConditionalWhere(request.EmotionType.HasValue, emotion => emotion.EmotionType == request.EmotionType)
            .ProjectTo<EmotionDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return emotions.ToQueryResult();
    }
}
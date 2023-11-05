using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class GetReactionEmotionsUseCase : BaseUseCase,
    IRequestHandler<GetReactionEmotionsQuery, QueryResult<EmotionDto>>
{
    public GetReactionEmotionsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<QueryResult<EmotionDto>> Handle(GetReactionEmotionsQuery request, CancellationToken cancellationToken)
    {
        var reaction = await DbContext.Reactions.FindAsync(request.ReactionId, cancellationToken);
        if (reaction is null)
        {
            throw new Exception();
        }
        var emotions = await DbContext.Emotions
            .Where(emotion => emotion.ReactionId == reaction.Id)
            .ProjectTo<EmotionDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return emotions.ToQueryResult();
    }
}
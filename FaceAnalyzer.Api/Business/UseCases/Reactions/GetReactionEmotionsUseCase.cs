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
    IRequestHandler<GetReactionEmotionsQuery, ExportReactionDto>
{
    public GetReactionEmotionsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ExportReactionDto> Handle(GetReactionEmotionsQuery request,
        CancellationToken cancellationToken)
    {
        var reactionExist =
            await DbContext.Reactions.AnyAsync(reaction => reaction.Id == request.ReactionId, cancellationToken);
        if (!reactionExist)
        {
            throw new EntityNotFoundException(nameof(Reaction), request.ReactionId);
        }

        var emotions = await DbContext.Reactions
            .Include(r => r.Emotions)
            .Where(reaction => reaction.Id == request.ReactionId)
            .ProjectTo<ExportReactionDto>(Mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        return emotions;
    }
}
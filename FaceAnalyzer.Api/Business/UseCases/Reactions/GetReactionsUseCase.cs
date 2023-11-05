using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;



namespace FaceAnalyzer.Api.Business.UseCases.Reactions;

public class GetReactionsUseCase : BaseUseCase, IRequestHandler<GetReactionsQuery, QueryResult<ReactionDto>>
{
    public async Task<QueryResult<ReactionDto>> Handle(GetReactionsQuery request,
        CancellationToken cancellationToken)
    {
        var results = await DbContext.Reactions
            .ProjectTo<ReactionDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return results.ToQueryResult();
    }

    public GetReactionsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}
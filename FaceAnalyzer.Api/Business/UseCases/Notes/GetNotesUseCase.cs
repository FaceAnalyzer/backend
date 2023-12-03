using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Notes;

public class GetNotesUseCase : BaseUseCase, IRequestHandler<GetNotesQuery, QueryResult<NoteDto>>
{
    public async Task<QueryResult<NoteDto>> Handle(GetNotesQuery request,
        CancellationToken cancellationToken)
    {
        var results = await DbContext.Notes
            .ConditionalWhere(request.Id.HasValue, n=> n.Id == request.Id)
            .ConditionalWhere(request.ExperimentId.HasValue, n=>n.ExperimentId == request.ExperimentId)
            .ProjectTo<NoteDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return results.ToQueryResult();
    }

    public GetNotesUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}
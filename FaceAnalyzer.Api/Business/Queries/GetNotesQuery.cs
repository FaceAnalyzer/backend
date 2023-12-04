using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetNotesQuery(int? Id, int? ExperimentId): IRequest<QueryResult<NoteDto>>;
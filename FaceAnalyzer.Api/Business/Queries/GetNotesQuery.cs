using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetNotesQuery(int? Id, string? Description, int? ExperimentId): IRequest<QueryResult<NoteDto>>;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Notes;

[SwaggerSchema(Title = nameof(CreateNoteCommand))]
public record CreateNoteCommand(string Description, int ExperimentId, int CreatorId): IRequest<NoteDto>;
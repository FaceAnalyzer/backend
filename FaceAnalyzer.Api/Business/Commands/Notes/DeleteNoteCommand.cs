using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Notes;

[SwaggerSchema(Title = nameof(DeleteNoteCommand))]
public record DeleteNoteCommand(int Id): IRequest;
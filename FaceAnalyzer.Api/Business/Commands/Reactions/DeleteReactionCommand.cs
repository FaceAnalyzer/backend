using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Reactions;

[SwaggerSchema(Title = nameof(DeleteReactionCommand))]
public record DeleteReactionCommand(int Id): IRequest;
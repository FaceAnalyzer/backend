using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Stimuli;

[SwaggerSchema(Title = nameof(DeleteStimuliCommand))]
public record DeleteStimuliCommand(int Id): IRequest;
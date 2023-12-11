using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Users;

[SwaggerSchema(Title = nameof(DeleteUserCommand))]
public record DeleteUserCommand(int Id): IRequest;
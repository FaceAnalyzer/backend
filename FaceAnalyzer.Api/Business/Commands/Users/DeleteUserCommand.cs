using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Users;

public record DeleteUserCommand(int Id): IRequest;
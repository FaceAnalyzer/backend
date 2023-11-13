using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Reactions;

public record DeleteReactionCommand(int Id): IRequest;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Reactions;

public record CreateReactionCommand(int StimuliId, string ParticipantName): IRequest<ReactionDto>;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Emotions;

public record CreateEmotionCommand
    (double Value, long TimeOffset, EmotionType EmotionType, int ReactionId) : IRequest<EmotionDto>;
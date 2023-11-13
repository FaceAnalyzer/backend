using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Stimuli;

public record DeleteStimuliCommand(int Id): IRequest;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Notes;

public record DeleteNoteCommand(int Id): IRequest;
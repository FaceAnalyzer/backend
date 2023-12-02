using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Notes;

public record CreateNoteCommand(string Description, int ExperimentId): IRequest<NoteDto>;
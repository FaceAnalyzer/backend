using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Notes;

public record EditNoteCommand(int Id, string Description, int? ExperimentId): IRequest<NoteDto>;
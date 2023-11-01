
using MediatR;

namespace FaceAnalyzer.Api.Business.Contracts;

public record ExperimentDto(int Id, string Name, string Description, int ProjectId);
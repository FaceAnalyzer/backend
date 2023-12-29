using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record ExportExperimentQuery(int ExperimentId, List<int>? StimuliIds): IRequest<ExportExperimentDto>;
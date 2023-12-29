
namespace FaceAnalyzer.Api.Business.Contracts;

public record ExportExperimentDto(int Id, string Name, int ProjectId, ICollection<ExportStimuliDto> Stimuli);
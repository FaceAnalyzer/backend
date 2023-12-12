
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateExperimentDtoExample : IExamplesProvider<CreateExperimentDto>
{
    public CreateExperimentDto GetExamples()
    {
        return new CreateExperimentDto(
            "Experiment Name",
            "A new experiment",
            1
        );
    }
}
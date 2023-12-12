
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class EditExperimentDtoExample : IExamplesProvider<EditExperimentDto>
{
    public EditExperimentDto GetExamples()
    {
        return new EditExperimentDto(
            "Updated experiment name",
            "Updated experiment name",
            1
        );
    }
}
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateStimuliDtoExample : IExamplesProvider<CreateStimuliDto>
{
    public CreateStimuliDto GetExamples()
    {
        return new CreateStimuliDto(
            "https://www.example.com",
            "Stimuli name",
            1,
            "Stimuli description"
        );
    }
}
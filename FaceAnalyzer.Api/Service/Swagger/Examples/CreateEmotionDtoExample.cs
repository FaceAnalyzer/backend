using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateEmotionDtoExample : IExamplesProvider<CreateEmotionDto>
{
    public CreateEmotionDto GetExamples()
    {
        return new CreateEmotionDto(
            .29229,
            1800,
            EmotionType.Anger,
            1
        );
    }
}
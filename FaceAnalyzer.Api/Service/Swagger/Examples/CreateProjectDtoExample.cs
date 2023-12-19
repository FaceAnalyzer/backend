using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateProjectDtoExample : IExamplesProvider<CreateProjectDto>
{
    public CreateProjectDto GetExamples()
    {
        return new CreateProjectDto(
            "Project Name"
        );
    }
}

using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class EditProjectDtoExample : IExamplesProvider<EditProjectDto>
{
    public EditProjectDto GetExamples()
    {
        return new EditProjectDto(
            "New Project Name"
        );
    }
}
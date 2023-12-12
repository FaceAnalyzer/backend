using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class ResetMyPasswordDtoExample : IExamplesProvider<ResetMyPasswordDto>
{
    public ResetMyPasswordDto GetExamples()
    {
        return new ResetMyPasswordDto(
            "Password"
        );
    }
}
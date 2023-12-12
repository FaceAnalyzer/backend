using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateNoteDtoExample : IExamplesProvider<CreateNoteDto>
{
    public CreateNoteDto GetExamples()
    {
        return new CreateNoteDto(
            "This is a note about an experiment",
            1,
            1
        );
    }
}
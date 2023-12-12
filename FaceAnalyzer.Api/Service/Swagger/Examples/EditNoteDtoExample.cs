using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class EditNoteDtoExample : IExamplesProvider<EditNoteDto>
{
    public EditNoteDto GetExamples()
    {
        return new EditNoteDto(
            "The note is now more exciting",
            1,
            1
        );
    }
}
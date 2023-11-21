using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateReactionDtoExample : IExamplesProvider<CreateReactionDto>
{
    public CreateReactionDto GetExamples()
    {
        var values = new Dictionary<EmotionType, double>();
        values.Add(EmotionType.Anger, .29323);
        values.Add(EmotionType.Disgust, .0323);
        values.Add(EmotionType.Fear, .009323);
        values.Add(EmotionType.Happiness, .29323);
        values.Add(EmotionType.Sadness, .0001);
        values.Add(EmotionType.Neutral, .00923);
        values.Add(EmotionType.Surprise, .100923);
        var readings = new EmotionReading(
            DateTime.UtcNow.Millisecond,
            values
        );

        return new CreateReactionDto(
            2,
            ParticipantName: "Algorithmy",
            EmotionReadings: new List<EmotionReading>
            {
                readings,
                readings,
                readings,
                readings,
                readings,
                readings
            }
        );
    }
}
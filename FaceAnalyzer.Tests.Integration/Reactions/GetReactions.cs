using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FaceAnalyzer.Tests.Integration.Reactions;

public class GetReactions
    : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public GetReactions(TestHostFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact(DisplayName = "user can get all reactions")]
    public async Task UserGetAllReactions()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);

        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);

        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);

        // Create a Reaction to get
        var reactions = new List<Reaction>
        {
            new()
            {
                ParticipantName = "Reaction 1",
                StimuliId = stimuli.Id
            },
            new()
            {
                ParticipantName = "Reaction 2",
                StimuliId = stimuli.Id
            },
            new()
            {
                ParticipantName = "Reaction 3",
                StimuliId = stimuli.Id
            },
            new()
            {
                ParticipantName = "Reaction 4",
                StimuliId = stimuli.Id
            },
        };
        await dbContext.Reactions.AddRangeAsync(reactions);
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await httpClient.GetStringAsync("reactions");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var responseReactions = JsonSerializer.Deserialize<QueryResult<ReactionDto>>(jsonResponse, jsonOptions);
        _testOutputHelper.WriteLine("Response Reactions: \n" + JsonSerializer.Serialize(responseReactions));

        responseReactions.Should().NotBeNull();
        responseReactions?.Count.Should().Be(reactions.Count);
        responseReactions?.Items.Should().NotBeNull();
        responseReactions?.Items.Should().NotBeEmpty();
        responseReactions?.Items.Count.Should().Be(responseReactions.Count);
        foreach (var reaction in responseReactions!.Items)
        {
            var dbReaction = reactions.Find(r =>
                r.ParticipantName == reaction.ParticipantName && r.StimuliId == reaction.StimuliId);
            reaction.ParticipantName.Should().Be(dbReaction?.ParticipantName);
            reaction.StimuliId.Should().Be(dbReaction?.StimuliId);
        }
    }

    [Fact(DisplayName = "User can successfully get a reaction by ID")]
    public async Task UserGetReactionById()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);

        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);

        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);

        // Create a Reaction to get
        var reaction = new Reaction
        {
            ParticipantName = "ExampleName",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);
        await dbContext.SaveChangesAsync();

        // Act
        var getResponse = await httpClient.GetStringAsync($"reactions/{reaction.Id}");


        // Get the reaction response
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var responseReaction = JsonSerializer.Deserialize<Reaction>(getResponse, jsonOptions);


        _testOutputHelper.WriteLine("Response Reaction: \n" + JsonSerializer.Serialize(responseReaction));

        // Assert
        responseReaction.Should().NotBeNull();
        responseReaction?.ParticipantName.Should().Be(reaction.ParticipantName);
        responseReaction?.StimuliId.Should().Be(reaction.StimuliId);
    }

    [Fact(DisplayName = "User can successfully get a emotion of a reaction by reactionID")]
    public async Task UserGetReactionEmotionById()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);

        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);

        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);

        var reaction = new Reaction
        {
            Id = 3,
            ParticipantName = "ExampleName",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);

        // Create a Reaction to get
        var emotion = new Emotion
        {
            Value = 50,
            EmotionType = EmotionType.Happiness,
            ReactionId = reaction.Id,
            TimeOffset = 405
        };
        dbContext.Emotions.Add(emotion);
        await dbContext.SaveChangesAsync();

        reaction.Should().NotBeNull();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions/{reaction.Id}/emotions");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await httpClient.GetStringAsync($"reactions/{reaction.Id}/emotions");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        var responseEmotion = JsonSerializer.Deserialize<ExportReactionDto>(jsonResponse, jsonOptions);
        _testOutputHelper.WriteLine("Response Emotion: \n" + JsonSerializer.Serialize(responseEmotion));

        responseEmotion.Should().NotBeNull();
        responseEmotion?.ParticipantName.Should().Be(reaction.ParticipantName);
        responseEmotion?.Id.Should().Be(reaction.Id);
        responseEmotion?.Emotions.Should().NotBeEmpty();
        responseEmotion?.Emotions.First().Should().NotBeNull();
        responseEmotion?.Emotions.First().EmotionType.Should().Be(emotion.EmotionType);
        responseEmotion?.Emotions.First().Value.Should().Be(emotion.Value);
        responseEmotion?.Emotions.First().TimeOffset.Should().Be(emotion.TimeOffset);
        responseEmotion?.Emotions.First().ReactionId.Should().Be(emotion.ReactionId);
    }

    [Fact(DisplayName = "User can successfully export emotions of a reaction by reactionID")]
    public async Task UserExportReactionEmotionById()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();
        var dbContext = _fixture.GetService<AppDbContext>();

        var project = new Project
        {
            Name = "Dummy Project"
        };
        dbContext.Projects.Add(project);

        var experiment = new Experiment
        {
            Name = "Dummy Experiment",
            Description = "Dummy description",
            ProjectId = project.Id
        };
        dbContext.Experiments.Add(experiment);

        var stimuli = new Stimuli
        {
            Link = "ExampleLink",
            ExperimentId = experiment.Id,
            Description = "FakeDescription",
            Name = "FakeName"
        };
        dbContext.Stimuli.Add(stimuli);

        // Create a Reaction to get
        var reaction = new Reaction
        {
            ParticipantName = "ExampleName",
            StimuliId = stimuli.Id
        };
        dbContext.Reactions.Add(reaction);

        // Create a Reaction to get
        var emotion = new List<Emotion>
        {
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Anger,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Happiness,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Disgust,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Fear,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Sadness,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Surprise,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
            new()
            {
                Value = 0.199,
                EmotionType = EmotionType.Neutral,
                ReactionId = reaction.Id,
                TimeOffset = 405
            },
        };
        await dbContext.Emotions.AddRangeAsync(emotion);
        await dbContext.SaveChangesAsync();

        reaction.Should().NotBeNull();

        // Act
        var getResponse = await httpClient.GetAsync($"reactions/{reaction.Id}/emotions/export");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseStream = await httpClient.GetStringAsync($"reactions/{reaction.Id}/emotions/export");
        responseStream.Should().NotBeNull();

        IList<EmotionCsv> records = new List<EmotionCsv>();
        using (var reader = new StringReader(responseStream))
        using (var csv = new CsvReader(reader,
                   new CsvConfiguration(CultureInfo.InvariantCulture)
                       { HeaderValidated = null, PrepareHeaderForMatch = args => args.Header.ToLower() }))
        {
            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                records.Add(new EmotionCsv
                {
                    TimeOffset = csv.GetField<long>("TimeOffset"),
                    Anger = csv.GetField<double>("Anger"),
                    Happiness = csv.GetField<double>("Happiness"),
                    Disgust = csv.GetField<double>("Disgust"),
                    Sadness = csv.GetField<double>("Sadness"),
                    Fear = csv.GetField<double>("Fear"),
                    Neutral = csv.GetField<double>("Neutral"),
                    Surprise = csv.GetField<double>("Surprise"),
                });
            }
        }

        records.Should().NotBeNull();
        records.Should().NotBeEmpty();
        foreach (var record in records)
        {
            record.Should().NotBeNull();
            record.TimeOffset.Should().Be(emotion.First().TimeOffset);
            record.Anger.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Anger).Value);
            record.Disgust.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Disgust).Value);
            record.Happiness.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Happiness).Value);
            record.Sadness.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Sadness).Value);
            record.Fear.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Fear).Value);
            record.Neutral.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Neutral).Value);
            record.Surprise.Should().Be(emotion.First(e => e.EmotionType == EmotionType.Surprise).Value);
        }
    }

    [Fact(DisplayName = "Getting a non-existing reaction returns 404")]
    public async Task GetNonExistingReactionReturns404()
    {
        // Arrange
        _fixture
            .EnableAuthentication()
            .AddDefaultPrincipal(1, UserRole.Admin)
            .Build();

        await _fixture.StartHost();
        var httpClient = _fixture.GetClient();

        // Act
        // Try to get a reaction that does not exist
        var getResponse = await httpClient.GetAsync($"reactions/{9999}"); // Assuming 9999 is a non-existing reaction ID

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
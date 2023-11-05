﻿using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Stimuli;

public record CreateStimuliCommand
    (
        string Link,
        string Description,
        int ExperimentId)
    : IRequest<StimuliDto>;
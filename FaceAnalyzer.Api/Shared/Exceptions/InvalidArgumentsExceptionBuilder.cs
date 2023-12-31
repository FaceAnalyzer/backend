﻿namespace FaceAnalyzer.Api.Shared.Exceptions;

public class InvalidArgumentsExceptionBuilder
{
    private readonly IDictionary<string, string> _messageDictionary = new Dictionary<string, string>();

    public InvalidArgumentsExceptionBuilder AddArgument(string argument, string message)
    {
        _messageDictionary.Add(argument, message);
        return this;
    }

    public bool HasArguments => _messageDictionary.Any();

    public InvalidArgumentsException Build()
    {
        var message = "";
        foreach (var kv in _messageDictionary)
        {
            message += $"{kv.Key}: {kv.Value}\n";
        }

        return new InvalidArgumentsException(message);
    }
}
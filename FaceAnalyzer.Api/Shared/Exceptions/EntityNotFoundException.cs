namespace FaceAnalyzer.Api.Shared.Exceptions;

public class EntityNotFoundException : Exception
{
    public string Entity { get; init; }
    public int Id { get; init; }

    public EntityNotFoundException(string entity, int id)
        : base($"Entity: {entity} with id: {id} was not found")
    {
        Entity = entity;
        Id = id;
    }
}
namespace FaceAnalyzer.Api.Shared.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entity, int id)
        : base($"Entity: {entity} with id: {id} was not found")
    {
        
    }
}
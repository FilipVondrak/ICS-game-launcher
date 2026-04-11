namespace ICSGameLauncher.Data.Exceptions;

public sealed class EntityNotFoundException(string entityName, int id) :
    Exception($"{entityName} with id {id} was not found.");
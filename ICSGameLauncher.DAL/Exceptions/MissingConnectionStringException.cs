namespace ICSGameLauncher.DAL.Exceptions;

public sealed class MissingConnectionStringException : Exception
{
    public MissingConnectionStringException(string name)
        : base($"Connection string '{name}' was not found.")
    {
    }
}
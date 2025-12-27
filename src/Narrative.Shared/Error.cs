namespace Narrative.Shared;

public sealed record Error
{
    public string Code { get; }

    public string Description { get; }

    internal ErrorType Type { get; }

    internal static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

    public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
}
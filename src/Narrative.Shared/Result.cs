using System.Diagnostics.CodeAnalysis;

namespace Narrative.Shared;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    internal Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
        {
            throw new ArgumentException("Invalid error.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
}

public sealed class Result<TValue> : Result
{
    internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) => Value = value;

    [NotNull]
    public TValue Value => IsSuccess
        ? field!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");
}
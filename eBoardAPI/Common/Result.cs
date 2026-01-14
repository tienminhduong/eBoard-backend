namespace eBoardAPI.Common;

public sealed class Result<TValue>
{
    public bool IsSuccess { get; init; }
    public TValue? Value { get; init; }
    public string? ErrorMessage { get; init; }
    
    public static Result<TValue> Success(TValue value) => new() { IsSuccess = true, Value = value };
    public static Result<TValue> Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
}
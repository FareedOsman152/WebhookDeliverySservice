namespace Shared;
public record Success(string ResponseMessage);
public record Failure(string[] Errors);
public union Result(Success, Failure);
public record Success<T>(T Data, string ResponseMessage);
public union Result<T>(Success<T>, Failure);
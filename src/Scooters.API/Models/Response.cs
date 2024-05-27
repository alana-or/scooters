namespace Scooters.Api.Models;

public class Response<T>
{
    public bool Success { get; internal set; }
    public string? Message { get; internal set; }
    public T? Data { get; internal set; }

    public static Response<T> CreateSuccess(T data, string message = "")
    {
        return new Response<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static Response<T> CreateFailure(string message)
    {
        return new Response<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}

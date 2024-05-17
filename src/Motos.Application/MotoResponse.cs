public class MotoResponse<T>
{
    public bool Success { get; internal set; }
    public string? Message { get; internal set; }
    public T? Data { get; internal set; }

    public static MotoResponse<T> CreateSuccessResponse(T data, string message = "")
    {
        return new MotoResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static MotoResponse<T> CreateFailureResponse(string message)
    {
        return new MotoResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}

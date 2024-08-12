namespace RefsGuy.Contracts.Responses;

public class FinalResponse<T>
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
namespace Personal.Api.Dtos;

public class AdminRequest<T>
{
    public required T Data { get; set; }
    public required string Key { get; set; }
}
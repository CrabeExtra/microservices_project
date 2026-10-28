namespace Contact.Application.Service.Interface;

public interface IApiService
{
    Task<bool> Fetch(ApiOptions apiOptions, CancellationToken cancellationToken);
}

public class ApiOptions
{
    public string Url { get; set; } = "";
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Subject { get; set; } = "";
    public string Email { get; set; } = "";
    public string Body { get; set; } = "";
}
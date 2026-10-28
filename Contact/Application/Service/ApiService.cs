
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Contact.Application.Service.Interface;
using DotNetEnv;

namespace Contact.Application.Service;

public class ApiService(
    ILogger<ApiService> logger,
    HttpClient httpClient,
    IConfiguration config
) : IApiService
{

    public async Task<bool> Fetch(ApiOptions apiOptions, CancellationToken cancellationToken)
    {
        try
        {

            
            return true;
        } catch(Exception error)
        {   
            logger.LogError("An error has occurred while attempting to run a fetch request.");
            logger.LogError(error.Message);
            return false;
        }
    }
}

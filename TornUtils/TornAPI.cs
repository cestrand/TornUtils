using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornUtils;
internal class TornAPI
{
    public Uri baseAddress = new Uri("https://api.torn.com");

    private HttpClientHandler handler;
    private string key;

    public TornAPI()
    {
        // setup configuration and credentials from environment variables
        key = System.Environment.GetEnvironmentVariable("TORN_API_KEY") 
            ?? throw new ConfigError("Set TORN_API_KEY environment variable.");

        handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (HttpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };
    }

    public async Task<HttpResponseMessage> Query(string user, string selections)
    {
        using (var client = new HttpClient(handler))
        {
            client.BaseAddress = baseAddress;
            HttpResponseMessage response = await client.GetAsync(
                $"/user/{user}?selections={selections}&key={key}");
            return response;
        }
    }
}

public class ConfigError : Exception
{
    public ConfigError(string s) : base(s) { }
}
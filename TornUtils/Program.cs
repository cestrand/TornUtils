using System.Net.Http.Headers;

namespace TornUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        static async Task InvokeRequestResponseService()
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (HttpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            using (var client = new HttpClient(handler))
            {
                string? key = System.Environment.GetEnvironmentVariable("TORN_API_KEY");
                client.BaseAddress = new Uri("https://api.torn.com");

                string user = "janepe";
                string selections = "attacks";
                HttpResponseMessage response = await client.GetAsync($"/user/{user}?selections={selections}&key={key}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());
                    var content = response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }

        }
    }
}
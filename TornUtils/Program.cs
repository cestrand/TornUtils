using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Text;

namespace TornUtils
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tornAPI = new TornAPI();
            HttpResponseMessage response = await tornAPI.Query("janepe", "attacks");
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
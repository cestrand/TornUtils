using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json.Nodes;

namespace TornUtils
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tornAPI = new CachedTornAPI();
            JsonNode response = await tornAPI.QueryJson("janepe", "attacks");
            Console.WriteLine(response.ToString());

        }
    }
}
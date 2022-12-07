using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TornUtils;
internal class CachedTornAPI : TornAPI
{
    SerializeCacher cache;
    public CachedTornAPI()
    {
        cache = new SerializeCacher();
    }

    public async new Task<JsonNode> QueryJson(string user, string selections, bool cached = true)
    {
        if (cached)
        {
            byte[]? cachedBytes = cache.Fetch(user, selections);
            if(cachedBytes is not null)
            {
                var str = System.Text.Encoding.UTF8.GetString(cachedBytes);
                JsonNode cachedNode = JsonNode.Parse(str)!;
                return cachedNode;
            }
        }

        JsonNode node = await base.QueryJson(user, selections);
        var outStr = node.ToJsonString();
        byte[] content = Encoding.UTF8.GetBytes(outStr);
        cache.Store(content, user, selections);
        return node;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SimpleBase;

namespace TornUtils;
internal class SerializeCacher
{
    DirectoryInfo dir;
    Base58 coder = new Base58(Base58Alphabet.Flickr);

    public SerializeCacher(string dirPath="cache")
    {
        dir = new DirectoryInfo(dirPath);
    }

    public string CalculateHash(params string[] vars)
    {
        string joined = string.Join(null, vars);
        string hash = coder.Encode(SHA256.HashData(Encoding.UTF8.GetBytes(joined)));
        return hash;
    }

    public byte[]? Fetch(params string[] vars)
    {
        string hash = CalculateHash(vars);

        DateTime? lastCachedTs = GetLastCachedTimestamp(hash);
        if (lastCachedTs is null)
        {
            return null;
        }
        DateTime ts = lastCachedTs.Value;
        if (DateTime.UtcNow - ts > TimeSpan.FromHours(1))
        {
            return null;
        }

        DirectoryInfo subdir = GetSubdir(hash);
        byte[] content = File.ReadAllBytes(Path.Combine(subdir.FullName, OutputDateTime(ts)));
        return content;
    }

    public void Store(byte[] content, params string[] vars)
    {
        string hash = CalculateHash(vars);
        DirectoryInfo subdir = GetSubdir(hash);
        if(!subdir.Exists)
        {
            subdir.Create();
        }
        File.WriteAllBytes(Path.Combine(subdir.FullName, OutputDateTime(DateTime.UtcNow)), content);
    }

    public DateTime? GetLastCachedTimestamp(string hash)
    {
        var subdir = GetSubdir(hash);
        if (!subdir.Exists)
        {
            return null;
        }

        DateTime latest = DateTime.MinValue;
        foreach (FileInfo file in subdir.EnumerateFiles())
        {
            string filename = Path.GetFileNameWithoutExtension(file.Name);
            // filename is timestamp
            DateTime ts = ParseDateTime(filename);
            if (ts > latest)
            {
                latest = ts;
            }
            return latest;
        }
        if (latest == DateTime.MinValue)
        {
            return null;
        }
        return latest;
    }

    private DirectoryInfo GetSubdir(string hash) => new(Path.Combine(dir.FullName, hash));

    public DateTime ParseDateTime(string s)
    {
        return DateTime.ParseExact(s, "yyyy-MM-dd-HH-mm-ss", null);
    }

    public string OutputDateTime(DateTime ts)
    {
        return ts.ToString("yyyy-MM-dd-HH-mm-ss");
    }


}

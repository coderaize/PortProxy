using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PortProxy
{
    public static class ConfigHandler
    {

        public static Dictionary<string, ProxyConfig>? GetMappedProxies
        {
            get
            {
                if (File.Exists("config.json"))
                {
                    var configJson = System.IO.File.ReadAllText("config.json");
                    return JsonSerializer.Deserialize<Dictionary<string, ProxyConfig>>(configJson);
                }
                else return new Dictionary<string, ProxyConfig>();
            }
        }

        public static string AddProxyMapped(string keyOrName, ProxyConfig proxyConfig)
        {
            var mp = GetMappedProxies ?? new Dictionary<string, ProxyConfig>();
            mp.Add(keyOrName, proxyConfig);
            var text = JsonSerializer.Serialize(mp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("config.json", text);
            return text;
        }

        public static string RemoveProxyMapped(string keyOrName, ProxyConfig proxyConfig)
        {
            var mp = GetMappedProxies ?? new Dictionary<string, ProxyConfig>();
            mp.Remove(keyOrName);
            var text = JsonSerializer.Serialize(mp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("config.json", text);
            return text;
        }

    }
}

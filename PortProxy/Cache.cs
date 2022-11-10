using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PortProxy
{
    public static class Cache
    {
        public static Configs Configs = new Configs();

        public static List<ActiveSession> ActiveSession = new List<ActiveSession>();
    }


    public class ActiveSession
    {
        public string ProxyName { get; set; }
        public ProxyConfig ProxyConfig { get; set; }
        public List<string> TcpClients { get; set; }
        public List<string> UdpClients { get; set; }
        public ProxyThread ProxyThread { get; set; }

        public ActiveSession(string proxyName, ProxyConfig proxyConfig, List<string> tcpClients, List<string> udpClients, ProxyThread proxyThread)
        {
            ProxyName = proxyName;
            ProxyConfig = proxyConfig;
            TcpClients = tcpClients;
            UdpClients = udpClients;
            ProxyThread = proxyThread;
        }
    }




    public class Configs : Dictionary<string, ProxyConfig>, IDisposable
    {
        public void Dispose()
        {
            this.Clear();
        }

        public Configs()
        {
            if (File.Exists("config.json"))
            {
                var configJson = System.IO.File.ReadAllText("config.json");
                var dic = JsonSerializer.Deserialize<Dictionary<string, ProxyConfig>>(configJson);
                if (dic != null)
                    foreach (string key in dic.Keys)
                    {
                        this.Add(key, dic[key]);
                    }
            }
        }

        public void RegisterSession(string proxyName, ProxyConfig proxyConfig)
        {
            this.Add(proxyName, proxyConfig);
            /// Updating local Config File
            var text = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("config.json", text);
        }

        public void UnRegisterSession(string proxyName)
        {
            this.Remove(proxyName);
            /// Updating local Config File
            var text = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("config.json", text);
        }

    }
}

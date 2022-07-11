using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortProxy
{


    public static class ProxyThreads
    {
        public static Dictionary<string, Thread> ProxyTaskThreads { get; set; } = new Dictionary<string, Thread>();

        public static Dictionary<string, List<string>> ActiveLinks { get; set; } = new Dictionary<string, List<string>>();

        public static Thread ProxyThread(string proxyName, ProxyConfig proxyConfig)
        {

            Thread PThread = new Thread(new ThreadStart(delegate
            {
                Task.WhenAll(StartProxy(proxyName, proxyConfig)).Wait();
            }));
            PThread.IsBackground = true;
            ProxyTaskThreads.Add(proxyName, PThread);
            return PThread;
        }

        public static void StopProxyThread(string proxyName)
        {

            try
            {
                StopProxy(proxyName);
                ProxyTaskThreads[proxyName].Abort();
            }
            catch { };
            ProxyTaskThreads.Remove(proxyName);
        }

        public static Dictionary<string, IEnumerable<Task>> ProxyTasks { get; set; } = new Dictionary<string, IEnumerable<Task>>();
        public static IEnumerable<Task> StartProxy(string proxyName, ProxyConfig proxyConfig)
        {

            var forwardPort = proxyConfig.forwardPort;
            var localPort = proxyConfig.localPort;
            var forwardIp = proxyConfig.forwardIp;
            var localIp = proxyConfig.localIp;
            var protocol = proxyConfig.protocol;
            Console.WriteLine($"{protocol} @ {localIp}:{localPort} to {forwardIp}:{forwardPort}");
            ActiveLinks.Add(proxyName, new List<string> { $"{protocol} @ {localIp}:{localPort} to {forwardIp}:{forwardPort}" });
            try
            {
                if (forwardIp == null)
                {
                    throw new Exception("forwardIp is null");
                }
                if (!forwardPort.HasValue)
                {
                    throw new Exception("forwardPort is null");
                }
                if (!localPort.HasValue)
                {
                    throw new Exception("localPort is null");
                }
                if (protocol != "udp" && protocol != "tcp" && protocol != "any")
                {
                    throw new Exception($"protocol is not supported {protocol}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start {proxyName} : {ex.Message}");
                throw;
            }

            bool protocolHandled = false;
            if (protocol == "udp" || protocol == "any")
            {
                protocolHandled = true;
                Task task;
                try
                {
                    var proxy = new UdpProxy();
                    task = proxy.Start(forwardIp, forwardPort.Value, localPort.Value, localIp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start {proxyName} : {ex.Message}");
                    throw;
                }

                yield return task;
            }

            if (protocol == "tcp" || protocol == "any")
            {
                protocolHandled = true;
                Task task;
                try
                {
                    var proxy = new TcpProxy();
                    task = proxy.Start(forwardIp, forwardPort.Value, localPort.Value, localIp);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start {proxyName} : {ex.Message}");
                    throw;
                }

                yield return task;
            }

            if (!protocolHandled)
            {
                throw new InvalidOperationException($"protocol not supported {protocol}");
            }

        }

        public static string StopProxy(string proxyName)
        {
            ActiveLinks.Remove(proxyName);

            IEnumerable<Task>? ProxyTask;
            if (ProxyTasks.TryGetValue(proxyName, out ProxyTask) == true)
            {
                ProxyTask.ToList().ForEach(X =>
                {
                    try { X.Dispose(); } catch { }
                });
                ProxyTasks.Remove(proxyName);
                return $"Proxy killed : {proxyName}";
            }
            else
            {
                return "No executing proxy found";
            }
        }
    }
}

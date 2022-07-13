using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortProxy
{


    public class ProxyThread
    {
        string ProxyName { get; set; }
        ProxyConfig ProxyConfig { get; set; }
        CancellationTokenSource tokenSource2 = new CancellationTokenSource();


        public ProxyThread(string proxyName, ProxyConfig proxyConfig)
        {
            ProxyName = proxyName;
            ProxyConfig = proxyConfig;
            ProxyTasks = GetProxyTasks();
        }

        public IEnumerable<Task> ProxyTasks { get; set; }

        public IEnumerable<Task> GetProxyTasks()
        {

            CancellationToken ct = tokenSource2.Token;
            //
            var forwardPort = ProxyConfig.forwardPort;
            var localPort = ProxyConfig.localPort;
            var forwardIp = ProxyConfig.forwardIp;
            var localIp = ProxyConfig.localIp;
            var protocol = ProxyConfig.protocol;
            Console.WriteLine($"{protocol} @ {localIp}:{localPort} to {forwardIp}:{forwardPort}");
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
                Console.WriteLine($"Failed to start {ProxyName} : {ex.Message}");
                throw;
            }

            bool protocolHandled = false;
            if (protocol == "udp" || protocol == "any")
            {
                protocolHandled = true;
                Task task;
                try
                {
                    var proxy = new UdpProxy(ProxyName);
                    task = proxy.Start(forwardIp, forwardPort.Value, localPort.Value, localIp, ct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start {ProxyName} : {ex.Message}");
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
                    var proxy = new TcpProxy(ProxyName);
                    task = proxy.Start(forwardIp, forwardPort.Value, localPort.Value, localIp, ct);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start {ProxyName} : {ex.Message}");
                    throw;
                }

                yield return task;
            }

            if (!protocolHandled)
            {
                throw new InvalidOperationException($"protocol not supported {protocol}");
            }

        }

        public void StartProxy()
        {
            new Thread(new ThreadStart(delegate
            {
                Task.WhenAll(ProxyTasks).Wait();
            })).Start();
        }


        public void Stop()
        {
            tokenSource2.Cancel();
            Task.Run(() =>
            {
                Cache.ActiveSession.RemoveAll(X => X.ProxyName == ProxyName);
            });
        }
    }
}

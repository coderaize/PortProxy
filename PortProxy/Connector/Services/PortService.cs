using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace PortProxy.Connector.Services
{
    public class PortService : PPConnector.PPConnectorBase
    {
        private readonly ILogger<PortService> _logger;
        public PortService(ILogger<PortService> logger)
        {
            _logger = logger;
        }


        public override Task<Reply> AddPort(PortRequest request, ServerCallContext context)
        {
            //////
            // perform task here
            //////
            return Task.FromResult(new Reply
            {
                Message = "reply " + request.LocalPort + "; - > " + request.ForwardPort+ "; -> " + request.ForwardIp
            }); ;
        }

    }
}

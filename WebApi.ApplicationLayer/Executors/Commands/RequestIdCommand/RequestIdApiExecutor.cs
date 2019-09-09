using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.ApplicationLayer.RabbitMQ;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors.Commands
{
    public class RequestIdApiExecutor : ExecutorCommandApi<RequestIdCommand>
    {
        private readonly RpcClient _rpcClient;
        private readonly ILogger<ClientApiExecutor> _logger;
        private readonly IRepositoryLog _repositoryLog;

        public RequestIdApiExecutor(RpcClient rpcClient, ILogger<ClientApiExecutor> logger, IRepositoryLog repositoryLog)
        {
            _rpcClient = rpcClient;
            _logger = logger;
            _repositoryLog = repositoryLog;
        }

        public override async Task<object> Execute(RequestIdCommand command)
        {
            return await Task.Run(() =>
            {
                var response = _rpcClient.Call(command);
                var log = JsonConvert.SerializeObject(response);
                _logger.LogInformation(log);
                _repositoryLog.Create(new Repository.Models.Log() { MassageLog = string.Format("RequestIdApiExecutor {0}", log) });
                return "status: " + response;
            });
        }

        public override IEnumerable<string> Validate(RequestIdCommand command)
        {
            if (command.request_id == 0)
            {
                yield return "request_id is not valid";
            }
        }
    }
}

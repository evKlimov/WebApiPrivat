using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.ApplicationLayer.Helpers;
using WebApi.ApplicationLayer.RabbitMQ;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors.Commands
{
    public class RequestClientApiExecutor : ExecutorCommandApi<RequestClientCommand>
    {
        private readonly RpcClient _rpcClient;
        private readonly ILogger<ClientApiExecutor> _logger;
        private readonly IRepositoryLog _repositoryLog;

        public RequestClientApiExecutor(RpcClient rpcClient, ILogger<ClientApiExecutor> logger, IRepositoryLog repositoryLog)
        {
            _rpcClient = rpcClient;
            _logger = logger;
            _repositoryLog = repositoryLog;
        }

        public override async Task<object> Execute(RequestClientCommand command)
        {
            return await Task.Run(() =>
            {
                var response = _rpcClient.Call(command);
                var log = JsonConvert.SerializeObject(response);
                _logger.LogInformation(log);
                _repositoryLog.Create(new Repository.Models.Log() { MassageLog = string.Format("RequestClientApiExecutor {0}", log) });
                return response;
            });
        }

        public override IEnumerable<string> Validate(RequestClientCommand command)
        {
            if (string.IsNullOrEmpty(command.client_id))
            {
                yield return "client_id is epmty";
            }
            else if (string.IsNullOrEmpty(command.departemnt_address))
            {
                yield return "departemnt_address is epmty";
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.ApplicationLayer.RabbitMQ;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors.Commands
{
    public class ClientApiExecutor : ExecutorCommandApi<ClientCommand>
    {
        private readonly RpcClient _rpcClient;
        private readonly ILogger<ClientApiExecutor> _logger;
        private readonly IRepositoryLog _repositoryLog;

        public ClientApiExecutor(RpcClient rpcClient, ILogger<ClientApiExecutor> logger, IRepositoryLog repositoryLog)
        {
            _rpcClient = rpcClient;
            _logger = logger;
            _repositoryLog = repositoryLog;
        }

        public override async Task<object> Execute(ClientCommand command)
        {
            return await Task.Run(() => 
            {
                var response = (Client)_rpcClient.Call(command);
                var log = JsonConvert.SerializeObject(response);
                _logger.LogInformation(log);
                _repositoryLog.Create(new Repository.Models.Log() { MassageLog = string.Format("ClientApiExecutor {0}", log)});
                return response.Id;
            });
        }

        public override IEnumerable<string> Validate(ClientCommand command)
        {
            if (string.IsNullOrEmpty(command.client_id))
            {
                yield return "client_id is epmty";
            }
            else if (string.IsNullOrEmpty(command.currency))
            {
                yield return "currency is epmty";
            }
            else if (string.IsNullOrEmpty(command.departemnt_address))
            {
                yield return "departemnt_address is epmty";
            }
        }
    }
}

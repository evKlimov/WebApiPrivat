using WebApi.ApplicationLayer.Helpers;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors.Commands
{
    public class RequestClientRabbitExecutor : ExecutorCommandRabbit<RequestClientCommand>
    {
        readonly IRepositoryClient _repositoryClient;

        public RequestClientRabbitExecutor(IRepositoryClient repositoryClient)
        {
            _repositoryClient = repositoryClient;
        }

        public override object Execute(RequestClientCommand command)
        {
            var client = _repositoryClient.FindBy(command.client_id, command.departemnt_address);
            return client;
        }
    }
}

using WebApi.ApplicationLayer.Helpers;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors.Commands
{
    public class ClientRabbitExecutor : ExecutorCommandRabbit<ClientCommand>
    {
        readonly IRepositoryClient _repositoryClient;

        public ClientRabbitExecutor(IRepositoryClient repositoryClient)
        {
            _repositoryClient = repositoryClient;
        }

        public override object Execute(ClientCommand command)
        {
            var client = new Client()
            {
                amount = command.amount,
                client_id = command.client_id,
                currency = command.currency,
                departemnt_address = command.departemnt_address
            };
            _repositoryClient.Create(client);
            return client;
        }
    }
}

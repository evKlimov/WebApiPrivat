using System.Text;
using WebApi.Repository;

namespace WebApi.ApplicationLayer.Executors
{
    public class RequestIdRabbitExecutor : ExecutorCommandRabbit<RequestIdCommand>
    {
        readonly IRepositoryClient _repositoryClient;

        public RequestIdRabbitExecutor(IRepositoryClient repositoryClient)
        {
            _repositoryClient = repositoryClient;
        }

        public override object Execute(RequestIdCommand command)
        {
            var client = _repositoryClient.FindById(command.request_id);
            return client.applicationStatus.ToString();
        }
    }
}

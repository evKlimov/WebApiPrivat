using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApplicationLayer;
using WebApi.ApplicationLayer.Executors;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly ApiExecutor _executor;

        public ClientController(ApiExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost("Add")]
        public Task<object> Add([FromBody]ClientCommand client)
        {
            return _executor.ApplyCommand(client);
        }

        [HttpPost("GetByRequestId")]
        public Task<object> GetByRequestId(int request_id)
        {
            return _executor.ApplyCommand(new RequestIdCommand() { request_id = request_id });
        }

        [HttpPost("GetByRequest")]
        public Task<object> GetByRequest(RequestClientCommand request)
        {
            return _executor.ApplyCommand(request);
        }
    }
}

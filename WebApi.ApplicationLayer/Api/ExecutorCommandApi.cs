using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.ApplicationLayer.Executors;

namespace WebApi.ApplicationLayer
{
    public abstract class ExecutorCommandApi<T> : IValidator<T>, IExecuteApi<T> where T : ICommand
    {
        public abstract Task<object> Execute(T command);

        public abstract IEnumerable<string> Validate(T command);
    }
}

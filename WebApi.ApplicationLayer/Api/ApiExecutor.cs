using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace WebApi.ApplicationLayer
{
    public class ApiExecutor
    {
        private readonly IServiceProvider _container;
        private readonly ILogger<ApiExecutor> _logger;

        public ApiExecutor(IServiceProvider container, ILogger<ApiExecutor> logger)
        {
            _container = container;
            _logger = logger;
        }

        public async Task<object> ApplyCommand<T>(T command) where T : ICommand
        {
            try
            {
                var result = string.Empty;
                var executor = _container.GetRequiredService<ExecutorCommandApi<T>>();
                if (executor != null)
                {
                    var validation = executor.Validate(command);
                    if (validation == null || !validation.Any())
                    {
                        return await executor.Execute(command);
                    }
                    else
                    {
                        result = string.Join(",", validation);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return ex.Message;
            }
        }
    }
}

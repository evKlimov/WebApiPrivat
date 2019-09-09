using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.ApplicationLayer
{
    public class RabbitExecutor
    {
        private readonly IServiceProvider _container;
        private readonly ILogger<ApiExecutor> _logger;

        public RabbitExecutor(IServiceProvider container, ILogger<ApiExecutor> logger)
        {
            _container = container;
            _logger = logger;
        }

        public object ApplyCommand<T>(T command) where T : class, ICommand
        {
            try
            {
                var executor = _container.GetRequiredService<ExecutorCommandRabbit<T>>();
                if (executor != null)
                {
                    return executor.Execute(command);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
            }
            return null;
        }
    }
}

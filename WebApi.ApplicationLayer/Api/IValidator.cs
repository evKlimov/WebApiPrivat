using System.Collections.Generic;

namespace WebApi.ApplicationLayer.Executors
{
    public interface IValidator<T>
    {
        IEnumerable<string> Validate(T command);
    }
}

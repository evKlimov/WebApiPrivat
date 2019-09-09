using System.Threading.Tasks;

namespace WebApi.ApplicationLayer
{
    public interface IExecuteApi<T>
    {
        Task<object> Execute(T command);
    }
}

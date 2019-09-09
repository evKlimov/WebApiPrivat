using System.Threading.Tasks;

namespace WebApi.ApplicationLayer
{
    public interface IExecuteRabbit<T>
    {
        object Execute(T command);
    }
}

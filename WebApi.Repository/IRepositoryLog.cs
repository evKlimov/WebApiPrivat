using WebApi.Repository.Models;

namespace WebApi.Repository
{
    public interface IRepositoryLog
    {
        void Create(Log log);
    }
}

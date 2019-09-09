using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    public interface IRepositoryClient
    {
        void Create(Client item);
        Client FindById(int id);
        IEnumerable<Client> FindBy(string client_id, string departemnt_address);
    }
}
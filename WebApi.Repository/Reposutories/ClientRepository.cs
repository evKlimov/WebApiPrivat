using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Repository
{
    public class ClientRepository : IRepositoryClient
    {
        private readonly SqlConnection _connection;

        public ClientRepository(ISession session)
        {
            _connection = session.Connection;
        }

        public void Create(Client item)
        {
            using (SqlCommand cmd = GetNewCommand())
            {
                var p = new DynamicParameters();
                p.Add("@client_id", item.client_id);
                p.Add("@departemnt_address", item.departemnt_address);
                p.Add("@amount", item.amount);
                p.Add("@applicationStatus", 0);
                p.Add("@currency", item.currency);
                int user = cmd.Connection.QueryFirst<int>("SetClient", p, commandType: CommandType.StoredProcedure);
                item.Id = user;
            }
        }

        public IEnumerable<Client> FindBy(string client_id, string departemnt_address)
        {
            using (SqlCommand cmd = GetNewCommand())
            {
                var p = new DynamicParameters();
                p.Add("@client_id", client_id);
                p.Add("@departemnt_address", departemnt_address);
                return cmd.Connection.Query<Client>("FindBy", p, commandType: CommandType.StoredProcedure);
            }
        }

        public Client FindById(int id)
        {
            using (SqlCommand cmd = GetNewCommand())
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                return cmd.Connection.QueryFirst<Client>("FindById", p, commandType: CommandType.StoredProcedure);
            }
        }

        SqlCommand GetNewCommand()
        {
            var command = new SqlCommand();
            command.Connection = _connection;
            return command;
        }
    }
}

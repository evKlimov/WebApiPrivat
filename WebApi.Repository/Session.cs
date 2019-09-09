using System.Data;
using System.Data.SqlClient;

namespace WebApi.Repository
{
    public class Session : ISession
    {
        public Session(SqlConnection connectionString)
        {
            Connection = connectionString;
        }

        public SqlConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}

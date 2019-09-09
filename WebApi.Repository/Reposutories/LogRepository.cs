using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WebApi.Repository.Models;

namespace WebApi.Repository
{
    public class LogRepository : IRepositoryLog
    {
        private readonly SqlConnection _connection;

        public LogRepository(ISession session)
        {
            _connection = session.Connection;
        }
        public void Create(Log log)
        {
            using (SqlCommand cmd = GetNewCommand())
            {
                var p = new DynamicParameters();
                p.Add("@MassageLog", log.MassageLog);
                int logId = cmd.Connection.QueryFirst<int>("SetLog", p, commandType: CommandType.StoredProcedure);
                log.Id = logId;
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

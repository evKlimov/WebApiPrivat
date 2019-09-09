using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApi.Repository
{
    public interface ISession : IDisposable
    {
        SqlConnection Connection { get; }
    }
}

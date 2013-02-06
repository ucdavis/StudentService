using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace StudentService.Services
{
    public class DbService : IDisposable
    {
        public readonly DbConnection Connection;

        public DbService()
        {
            Connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MainDb"].ConnectionString;);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
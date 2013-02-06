using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace StudentService.Helpers
{
    public class DbManager : IDisposable
    {
        public readonly DbConnection Connection;

        public DbManager()
        {
            Connection = new SqlConnection(WebConfigurationManager.ConnectionStrings[""].ConnectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
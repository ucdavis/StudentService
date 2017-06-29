using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace StudentService.Helpers
{
    public class CommencementDbManager : IDisposable
    {
        public readonly DbConnection Connection;

        public CommencementDbManager()
        {
            Connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["CommencementDB"].ConnectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
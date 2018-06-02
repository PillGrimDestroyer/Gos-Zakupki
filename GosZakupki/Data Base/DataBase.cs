using System.Data;
using System.Data.SqlClient;

namespace GosZakupki.Data_Base
{
    class DataBase
    {
        protected string connectionString { get; set; }

        public DataBase()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-2VH5U9S\SQLEXPRESS",
                InitialCatalog = @"Gos_zakupki",
                IntegratedSecurity = true
            };
            connectionString = connectionStringBuilder.ToString();
        }

        public SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection;
        }
    }
}

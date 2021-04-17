using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DPWALab2Periodo2.Models
{
    public class Connection
    {
        SqlConnection connection;

        string connectionString =
            @"Data Source = localhost\SQLEXPRESS;
            Initial Catalog = bank;
            Persist Security Info=True;
            Integrated Security = true;";

        public void dbConnection()
        {
            connection = new SqlConnection(connectionString);
            try { connection.Open(); }
            catch (Exception ex)
            {
                Debug.WriteLine($"Not working bruh {ex}");
            }
        }

        public bool executeQuery(String query)
        {
            dbConnection();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            try
            {
                int code = cmd.ExecuteNonQuery();
                connection.Close();
                return code > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                connection.Close();
                return false;
            }
        }

        public SqlDataReader reader(String query)
        {
            dbConnection();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            SqlDataReader sqlReader;
            try
            {
                sqlReader = cmd.ExecuteReader();
                return sqlReader;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                connection.Close();
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWinform.Model
{
    internal class ConnectDB
    {
        private readonly string connectionString = @"Data Source=TRUONGSON\SQLEXPRESS01;Initial Catalog=QuanLyDiemSV;Integrated Security=True;";
        private SqlConnection connection;

        public ConnectDB()
        {
            this.connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    Console.WriteLine("Kết nối thành công.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi kết nối: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Đóng kết nối thành công.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đóng kết nối: " + ex.Message);
            }
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

    }
}

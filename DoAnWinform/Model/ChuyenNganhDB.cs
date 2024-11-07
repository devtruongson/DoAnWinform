using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DoAnWinform.Model
{
    internal class ChuyenNganhDB
    {
        public bool InsertData(string ten, bool isDeleted)
        {
            try
            {
                ConnectDB connect = new ConnectDB();
                connect.OpenConnection();
                string query = "INSERT INTO ChuyenNganh (TenCN, DaXoa) " + "VALUES (@TenCN, @DaXoa)";

                using (SqlCommand cmd = new SqlCommand(query, connect.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@TenCN", ten);
                    cmd.Parameters.AddWithValue("@DaXoa", isDeleted);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex){
                Console.WriteLine("Lỗi khi thêm DOCGIA: " + ex.Message);
                return false;
            }

        }

        public bool UpdateData(int id ,string ten, bool isDeleted)
        {
            try
            {
                ConnectDB connect = new ConnectDB();
                connect.OpenConnection();
                string query = "UPDATE ChuyenNganh set TenCN = @TenCN, DaXoa = @DaXoa where id = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connect.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@TenCN", ten);
                    cmd.Parameters.AddWithValue("@DaXoa", isDeleted);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cap nhat DOCGIA: " + ex.Message);
                return false;
            }

        }

        public bool DeleteData(int id)
        {
            try
            {
                ConnectDB connect = new ConnectDB();
                connect.OpenConnection();
                string query = "Delete from ChuyenNganh where id = @ID";

                using (SqlCommand cmd = new SqlCommand(query, connect.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xoa DOCGIA: " + ex.Message);
                return false;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnWinform.Model
{
    internal class SinhVienDB
    {
        public bool InsertStudent(
            string hoTen,
            string email,
            string soDienThoai,
            string studentCode,
            int chuyenNganh,
            string gioiTinh,
            DateTime ngaySinh,
            string diaChi,
            string khoaHoc,
            string cccd,
            bool daXoa){
                    try
                    {
                        ConnectDB connect = new ConnectDB();
                        connect.OpenConnection();
                        string query = "INSERT INTO SinhVien (HoTen, Email, SoDienThoai, StudentCode, ChuyenNghanh, GioiTinh, NgaySinh, DiaChi, KhoaHoc, CCCD, DaXoa) " +
                                       "VALUES (@HoTen, @Email, @SoDienThoai, @StudentCode, @ChuyenNghanh, @GioiTinh, @NgaySinh, @DiaChi, @KhoaHoc, @CCCD, @DaXoa)";

                        using (SqlCommand cmd = new SqlCommand(query, connect.GetConnection()))
                        {
                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                            cmd.Parameters.AddWithValue("@StudentCode", studentCode);
                            cmd.Parameters.AddWithValue("@ChuyenNghanh", chuyenNganh);
                            cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                            cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                            cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                            cmd.Parameters.AddWithValue("@KhoaHoc", khoaHoc);
                            cmd.Parameters.AddWithValue("@CCCD", cccd);
                            cmd.Parameters.AddWithValue("@DaXoa", daXoa);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi thêm SinhVien: " + ex.Message);
                        return false;
                    }
                }
        }
}

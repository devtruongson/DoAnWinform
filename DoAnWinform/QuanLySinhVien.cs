using DoAnWinform.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnWinform
{
    public partial class QuanLySinhVien : Form
    {

        private string queryActive = "select SinhVien.Ma, HoTen, Email, SoDienThoai, StudentCode, GioiTinh, NgaySinh, DiaChi, KhoaHoc, CCCD, CASE WHEN SinhVien.DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa, TenCN as ChuyenNghanh from SinhVien left join ChuyenNganh on ChuyenNganh.id = SinhVien.ChuyenNghanh ";
        private SqlConnection con;
        Dictionary<int, string> listCN = new Dictionary<int, string>();
        string[] provinces = new string[]
        {
             "--- Chọn Địa Chỉ ---", "An Giang", "Bà Rịa-Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bến Tre", "Bình Định",
            "Bình Dương", "Bình Phước", "Bình Thuận", "Cà Mau", "Cao Bằng", "Cần Thơ",
            "Cao Bằng", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai",
            "Đồng Tháp", "Hà Giang", "Hà Nam", "Hà Nội", "Hà Tĩnh", "Hải Dương",
            "Hải Phòng", "Hòa Bình", "Hồ Chí Minh", "Hậu Giang", "Hưng Yên", "Khánh Hòa",
            "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai",
            "Long An", "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ",
            "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh",
            "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên",
            "Thanh Hóa", "Thừa Thiên-Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang",
            "Vĩnh Long", "Vĩnh Phúc", "Yên Bái"
        };
        string[] khoaHoc = new string[]
        {
            "--- Chọn Khóa Học ---","Khóa 1", "Khóa 2", "Khóa 3", "Khóa 4", "Khóa 5", "Khóa 6", "Khóa 7", "Khóa 8", "Khóa 9", "Khóa 10",
            "Khóa 11", "Khóa 12", "Khóa 13", "Khóa 14", "Khóa 15", "Khóa 16", "Khóa 17", "Khóa 18", "Khóa 19", "Khóa 20"
        };
        string[] monHoc = new string[]
        {
            "--- Chọn Môn Học ---","Khóa 1", "Khóa 2", "Khóa 3", "Khóa 4", "Khóa 5", "Khóa 6", "Khóa 7", "Khóa 8", "Khóa 9", "Khóa 10",
            "Khóa 11", "Khóa 12", "Khóa 13", "Khóa 14", "Khóa 15", "Khóa 16", "Khóa 17", "Khóa 18", "Khóa 19", "Khóa 20"
        };



        public QuanLySinhVien()
        {
            ConnectDB connect = new ConnectDB();
            this.con = connect.GetConnection();
            InitializeComponent();
        }

        private void QuanLySinhVien_Load(object sender, EventArgs e)
        {
            this.setupComboChuyenNganh();
            this.showTable(queryActive);
        }

        public void showTable(String query)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            gridViewListSV.AllowUserToAddRows = false;
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridViewListSV.DataSource = dt;
            gridViewListSV.Refresh();
            if (!gridViewListSV.Columns.Contains("btnXoaMem") && !gridViewListSV.Columns.Contains("btnSua"))
            {
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "btnSua";
                buttonColumn1.HeaderText = "Cập Nhật";
                buttonColumn1.Text = "Cập Nhật";
                buttonColumn1.UseColumnTextForButtonValue = true;
                gridViewListSV.Columns.Add(buttonColumn1);

                DataGridViewButtonColumn buttonColumn2 = new DataGridViewButtonColumn();
                buttonColumn2.Name = "btnXoaMem";
                buttonColumn2.HeaderText = "Xóa";
                buttonColumn2.Text = "Trash";
                buttonColumn2.UseColumnTextForButtonValue = true;
                gridViewListSV.Columns.Add(buttonColumn2);
            }
            gridViewListSV.Columns["btnSua"].DisplayIndex = gridViewListSV.ColumnCount - 2;
            gridViewListSV.Columns["btnXoaMem"].DisplayIndex = gridViewListSV.ColumnCount - 1;
        }

        private void setupComboChuyenNganh()
        {
            string query = "SELECT ID, TenCN FROM ChuyenNganh WHERE DaXoa = 0"; 
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            this.listCN.Add(-1, "---- Chọn Chuyên Nghành ----");
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string tenCN = reader.GetString(1);
                listCN.Add(id, tenCN); 
            }
            reader.Close();

            cbChuyenNghanhThem.DataSource = new BindingSource(listCN, null);
            cbChuyenNghanhThem.DisplayMember = "Value"; // Hiển thị tên chuyên ngành
            cbChuyenNghanhThem.ValueMember = "Key";     // Sử dụng ID làm giá trị
            cbLocChuyenNganh.DataSource = new BindingSource(listCN, null);
            cbLocChuyenNganh.DisplayMember = "Value"; 
            cbLocChuyenNganh.ValueMember = "Key";
            cbDiaChiFilter.DataSource = new BindingSource(provinces, null);
            cbLocKhoaHoc.DataSource = new BindingSource(khoaHoc, null);
            cbLocMonHoc.DataSource = new BindingSource(monHoc, null);
            cbKhoaHocThem.DataSource = new BindingSource(khoaHoc, null);
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = this.messageResult.Show();

            if (result == DialogResult.Yes)
            {
                this.resetFiled();
            }
        }

        private void resetFiled ()
        {
            txtHoTenThem.Text = "";
            txtSDTThem.Text = "";
            txtEmailThem.Text = "";
            cbChuyenNghanhThem.SelectedIndex = 0;
            cbGioiTinhThem.SelectedIndex = 0;
            txtDiaChiThem.Text = "";
            txtCCCDThem.Text = "";
            cbKhoaHocThem.SelectedIndex = 0;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabTwo_Click(object sender, EventArgs e)
        {

        }

        private void tabOne_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            bool isValid = this.validate();
            if (!isValid)
            {
                return;
            }

            DialogResult result = this.messageResult.Show("Bạn chắc chắn muốn thêm sinh viên này?");
            if (result == DialogResult.Yes)
            {
                this.resetFiled();
            }

            string studentCode = txtStudentCodeThem.Text;
            string hoTen = txtHoTenThem.Text;
            string email = txtEmailThem.Text;
            string sdt = txtSDTThem.Text;
            string khoaHoc = cbKhoaHocThem.SelectedItem.ToString();
            int chuyenNganh = Convert.ToInt32(cbChuyenNghanhThem.SelectedValue.ToString());
            string gioiTinh = cbGioiTinhThem.SelectedItem.ToString();
            DateTime ngaySinh = datePickNgaySinhThem.Value;
            string diaChi = txtDiaChiThem.Text;
            string trangThai = cbHienThiThem.SelectedItem.ToString();
            string CCCD = txtCCCDThem.Text;
           
            SinhVienDB sinhVienConnect = new SinhVienDB();
            bool isInsertData = sinhVienConnect.InsertStudent(hoTen, email, sdt, studentCode, chuyenNganh , gioiTinh, ngaySinh, diaChi, khoaHoc, CCCD, trangThai != "Hiển Thị");

            if(isInsertData )
            {
                this.messageResult.Show("Bạn đã thêm thành công sinh viên!");
                this.resetFiled();
            }
            else
            {
                this.messageResult.Show("Có lỗi xảy ra khi thêm sinh viên!");
            }

        }

        private bool validate()
        {
            bool isValid = true;
          

            if (string.IsNullOrWhiteSpace(txtStudentCodeThem.Text))
            {
                this.messageResult.Show("Vui lòng nhập mã sinh viên.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTenThem.Text))
            {
                this.messageResult.Show("Vui lòng nhập họ tên.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmailThem.Text) || !IsValidEmail(txtEmailThem.Text))
            {
                this.messageResult.Show("Vui lòng nhập email hợp lệ.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtSDTThem.Text) || txtSDTThem.Text.Length > 12)
            {
                this.messageResult.Show("Vui lòng nhập số điện thoại hợp lệ (10 chữ số).");
                isValid = false;
            }

            if (cbKhoaHocThem.SelectedItem == null || cbKhoaHocThem.SelectedIndex == 0)
            {
                this.messageResult.Show("Vui lòng chọn khóa học.");
                isValid = false;
            }

            if (cbChuyenNghanhThem.SelectedValue == null || cbChuyenNghanhThem.SelectedIndex == 0)
            {
                this.messageResult.Show("Vui lòng chọn chuyên ngành.");
                isValid = false;
            }

            if (cbGioiTinhThem.SelectedItem == null || cbGioiTinhThem.SelectedIndex == 0)
            {
                this.messageResult.Show("Vui lòng chọn giới tính.");
                isValid = false;
            }

            if (datePickNgaySinhThem.Value >= DateTime.Now)
            {
                this.messageResult.Show("Ngày sinh không hợp lệ.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChiThem.Text))
            {
                this.messageResult.Show("Vui lòng nhập địa chỉ.");
                isValid = false;
            }

            if (cbHienThiThem.SelectedItem == null || cbHienThiThem.SelectedIndex == 0)
            {
                this.messageResult.Show("Vui lòng chọn trạng thái hiển thị.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtCCCDThem.Text) || txtCCCDThem.Text.Length != 12)
            {
                this.messageResult.Show("Vui lòng nhập số CCCD hợp lệ (12 chữ số).");
                isValid = false;
            }

            return isValid;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(guna2TabControl1.SelectedIndex)
            {
                case 0:
                    {
                        this.showTable(queryActive);
                        break;
                    }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtTimKiemSV.Text.Trim()))
            {
                this.messageResult.Show("Vui lòng nhập tên để tìm kiếm!");
                return;
            }
            showTable(queryActive + "where SinhVien.HoTen like '%" + txtTimKiemSV.Text.Trim() + "%'");

        }

        private void txtTimKiemSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLoadAgainDataList_Click(object sender, EventArgs e)
        {
            showTable(queryActive);
            this.cbLocChuyenNganh.SelectedIndex = 0;
            this.cbLocKhoaHoc.SelectedIndex = 0;
            this.cbDiaChiFilter.SelectedIndex = 0;
        }

        private void txtTimKiemSV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (string.IsNullOrEmpty(txtTimKiemSV.Text.Trim()))
                {
                    this.messageResult.Show("Vui lòng nhập tên để tìm kiếm!");
                    return;
                }
                showTable(queryActive + "where SinhVien.HoTen like '%" + txtTimKiemSV.Text.Trim() + "%'");
            }
        }

        private void btnLocDanhSach_Click(object sender, EventArgs e)
        {
            string ChuyenNghanh = cbLocChuyenNganh.SelectedValue.ToString();
            string diaChi = cbDiaChiFilter.SelectedItem.ToString();
            string khoaHoc = cbLocKhoaHoc.SelectedItem.ToString();

            string newQueryFlter = this.queryActive + "where Ma > -1";

            if(!string.IsNullOrEmpty(ChuyenNghanh) && cbLocChuyenNganh.SelectedIndex > 0)
            {
                newQueryFlter += " and ChuyenNghanh = " + ChuyenNghanh;
            }

            if (!string.IsNullOrEmpty(diaChi) && cbDiaChiFilter.SelectedIndex > 0)
            {
                newQueryFlter += " and DiaChi like '%" + diaChi + "%'";
            }

            if (!string.IsNullOrEmpty(khoaHoc) && cbLocKhoaHoc.SelectedIndex > 0)
            {
                newQueryFlter += " and KhoaHoc like '%" + khoaHoc + "%'";
            }

            this.showTable(newQueryFlter);
        }
    }
}

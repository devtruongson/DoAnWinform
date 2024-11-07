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
            this.showTable("select * from SinhVien");
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
            gridViewListSV.Refresh();
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

            cbChuyenNghanh.DataSource = new BindingSource(listCN, null);
            cbChuyenNghanh.DisplayMember = "Value"; // Hiển thị tên chuyên ngành
            cbChuyenNghanh.ValueMember = "Key";     // Sử dụng ID làm giá trị
            cbLocChuyeNganh.DataSource = new BindingSource(listCN, null);
            cbLocChuyeNganh.DisplayMember = "Value"; 
            cbLocChuyeNganh.ValueMember = "Key";
            cbDiaChiFilter.DataSource = new BindingSource(provinces, null);
            cbLocKhoaHoc.DataSource = new BindingSource(khoaHoc, null);
            cbLocMonHoc.DataSource = new BindingSource(monHoc, null);
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
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            cbChuyenNghanh.SelectedIndex = 0;
            cbGioiTinh.SelectedIndex = 0;
            txtDiaChi.Text = "";
            txtKhoaHoc.Text = "";
            txtCCCD.Text = "";
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
    }
}

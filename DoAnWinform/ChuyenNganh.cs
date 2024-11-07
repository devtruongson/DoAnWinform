using DoAnWinform.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnWinform
{
    public partial class ChuyenNganh : Form
    {
        private SqlConnection con;
        private DataTable dt;
        public ChuyenNganh()
        {
            ConnectDB connect = new ConnectDB();
            this.con = connect.GetConnection();
            InitializeComponent();
        }

        private void ChuyenNganh_Load(object sender, EventArgs e)
        {
            this.showTable("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 0");
        }

        public void showTable(String query)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            gridViewListCN.AllowUserToAddRows = false;
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (!gridViewListCN.Columns.Contains("btnXoaMem") && !gridViewListCN.Columns.Contains("btnSua"))
            {
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "btnSua";
                buttonColumn1.HeaderText = "Cập Nhật";
                buttonColumn1.Text = "Cập Nhật";
                buttonColumn1.UseColumnTextForButtonValue = true;
                gridViewListCN.Columns.Add(buttonColumn1);

                DataGridViewButtonColumn buttonColumn2 = new DataGridViewButtonColumn();
                buttonColumn2.Name = "btnXoaMem";
                buttonColumn2.HeaderText = "Xóa";
                buttonColumn2.Text = "Trash";
                buttonColumn2.UseColumnTextForButtonValue = true;
                gridViewListCN.Columns.Add(buttonColumn2);
            }

            gridViewListCN.DataSource = dt;
            gridViewListCN.Refresh();
        }

        public void showTableTrash(String query)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            gridViewTrash.AllowUserToAddRows = false;
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (!gridViewTrash.Columns.Contains("btnXoaMem") && !gridViewTrash.Columns.Contains("btnSua") && !gridViewTrash.Columns.Contains("btnDelete"))
            {
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "btnSua";
                buttonColumn1.HeaderText = "Cập Nhật";
                buttonColumn1.Text = "Cập Nhật";
                buttonColumn1.UseColumnTextForButtonValue = true;
                gridViewTrash.Columns.Add(buttonColumn1);

                DataGridViewButtonColumn buttonColumn2 = new DataGridViewButtonColumn();
                buttonColumn2.Name = "btnKhoiPhuc";
                buttonColumn2.HeaderText = "Khôi Phục";
                buttonColumn2.Text = "Restore";
                buttonColumn2.UseColumnTextForButtonValue = true;
                gridViewTrash.Columns.Add(buttonColumn2);

                DataGridViewButtonColumn buttonColumn3 = new DataGridViewButtonColumn();
                buttonColumn3.Name = "btnDelete";
                buttonColumn3.HeaderText = "Xóa Force";
                buttonColumn3.Text = "Xóa Force";
                buttonColumn3.UseColumnTextForButtonValue = true;
                gridViewTrash.Columns.Add(buttonColumn3);
            }

            gridViewTrash.DataSource = dt;
            gridViewTrash.Refresh();
        }

        private void gridViewListCN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
   
            if (e.ColumnIndex == gridViewListCN.Columns["btnSua"].Index && e.RowIndex >= 0)
            {
                DialogResult result = this.messageResult.Show("Bạn Chắc Chắn Muốn Sửa?");

                if (result == DialogResult.Yes)
                {
                    this.UpdateChuyenNganh(e.RowIndex, "notrash");
                }
            }
            else if (e.ColumnIndex == gridViewListCN.Columns["btnXoaMem"].Index && e.RowIndex >= 0)
            {
                DialogResult result = this.messageResult.Show("Bạn Chắc Chắn Muốn Xóa?");

                if (result == DialogResult.Yes)
                {
                    this.UpdateChuyenNganh(e.RowIndex, "trash");
                }
            }
        }

        private void gridViewTrash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == gridViewTrash.Columns["btnSua"].Index && e.RowIndex >= 0)
            {
                DialogResult result = this.messageResult.Show("Bạn Chắc Chắn Muốn Sửa?");

                if (result == DialogResult.Yes)
                {
                    this.UpdateChuyenNganhTrash(e.RowIndex, "norestore");
                }
            }
            else if (e.ColumnIndex == gridViewTrash.Columns["btnKhoiPhuc"].Index && e.RowIndex >= 0)
            {
                DialogResult result = this.messageResult.Show("Bạn Chắc Chắn Muốn khôi Phục?");

                if (result == DialogResult.Yes)
                {
                    this.UpdateChuyenNganhTrash(e.RowIndex, "restore");
                }
            }

            else if (e.ColumnIndex == gridViewTrash.Columns["btnDelete"].Index && e.RowIndex >= 0)
            {
                DialogResult result = this.messageResult.Show("Bạn Chắc Chắn Muốn Xóa Không Lưu Ý Không Thể Khôi Phục?");
                if (result == DialogResult.Yes)
                {
                    int ID = Convert.ToInt32(gridViewTrash.Rows[e.RowIndex].Cells[3].Value.ToString());
                    ChuyenNganhDB chuyenNganh = new ChuyenNganhDB();
                    bool isDelete = chuyenNganh.DeleteData(ID);
                    if (isDelete)
                    {
                        this.messageDialog.Show("Bạn đã xóa thành công!");
                        this.showTableTrash("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 1");
                    }
                    else
                    {
                        this.messageDialog.Show("Không thể xóa vì hiện đang có SV theo học chuyên nghành này");
                    }
                }
            }
        }

        private void UpdateChuyenNganh(int rowClicked, string type)
        {

            int ID = Convert.ToInt32(gridViewListCN.Rows[rowClicked].Cells[2].Value.ToString());
            string TenCN = gridViewListCN.Rows[rowClicked].Cells[3].Value.ToString();
            bool isDeleted = bool.TryParse(gridViewListCN.Rows[rowClicked].Cells[4].Value.ToString(), out isDeleted) ? isDeleted : false;

            ChuyenNganhDB chuyenNganh = new ChuyenNganhDB();

            switch (type)
            {
                case "trash":
                    {
                        bool isUpdated = chuyenNganh.UpdateData(ID, TenCN, true);
                        if (isUpdated)
                        {
                            this.messageDialog.Show("Chuyên Nghành Đã Đưa Vào Thùng Rác thành công!");
                            this.showTable("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 0");
                        }
                        else
                        {
                            this.messageDialog.Show("Có lỗi khi cập nhật!");
                        }
                        break;
                    }
                case "notrash":
                    {
                        bool isUpdated = chuyenNganh.UpdateData(ID, TenCN, false);
                        if (isUpdated)
                        {
                            this.messageDialog.Show("Bạn đã cập nhật thành công!");
                            this.showTable("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 0");
                        }
                        else
                        {
                            this.messageDialog.Show("Có lỗi khi cập nhật!");
                        }
                        break;
                    }
            }
        }
        
        private void UpdateChuyenNganhTrash(int rowClicked, string type)
        {

            int ID = Convert.ToInt32(gridViewTrash.Rows[rowClicked].Cells[3].Value.ToString());
            string TenCN = gridViewTrash.Rows[rowClicked].Cells[4].Value.ToString();
            bool isDeleted = bool.TryParse(gridViewTrash.Rows[rowClicked].Cells[5].Value.ToString(), out isDeleted) ? isDeleted : false;

            ChuyenNganhDB chuyenNganh = new ChuyenNganhDB();

            switch (type)
            {
                case "restore":
                    {
                        bool isUpdated = chuyenNganh.UpdateData(ID, TenCN, false);
                        if (isUpdated)
                        {
                            this.messageDialog.Show("Chuyên Nghành Đã Khôi Phục thành công!");
                            this.showTableTrash("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 1");
                        }
                        else
                        {
                            this.messageDialog.Show("Có lỗi khi cập nhật!");
                        }
                        break;
                    }
                case "norestore":
                    {
                        bool isUpdated = chuyenNganh.UpdateData(ID, TenCN, true);
                        if (isUpdated)
                        {
                            this.messageDialog.Show("Bạn đã cập nhật thành công!");
                            this.showTableTrash("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 1");
                        }
                        else
                        {
                            this.messageDialog.Show("Có lỗi khi cập nhật!");
                        }
                        break;
                    }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(tabControl.SelectedIndex)
            {
                case 0:
                    {
                        this.showTable("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 0");
                        break;
                    }
                case 2:
                    {
                        this.showTableTrash("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 1");  
                        break;
                    }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.showTableTrash("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 1");
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.showTable("SELECT id, TenCN, CASE WHEN DaXoa = 0 THEN 'Active' ELSE 'Trash' END AS DaXoa FROM ChuyenNganh WHERE DaXoa = 0");
        }

        private void btnTimKiemCN_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtTimKiemCN.Text))
            {
                this.messageDialog.Show("Vui lòng nhật tên để tìm kiếm.... ");
                return;
            }

            this.showTable("select * from ChuyenNganh where DaXoa = 0 and TenCN like '%" + txtTimKiemCN.Text + "%'");
        }

        private void txtTimKiemCN_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTimKiemCN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (string.IsNullOrEmpty(txtTimKiemCN.Text))
                {
                    this.messageDialog.Show("Vui lòng nhật tên để tìm kiếm.... ");
                    return;
                }
                this.showTable("select * from ChuyenNganh where DaXoa = 0 and TenCN like '%" + txtTimKiemCN.Text + "%'");
            }
        }

        private void btnTimKiemTrash_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemTrash.Text))
            {
                this.messageDialog.Show("Vui lòng nhật tên để tìm kiếm.... ");
                return;
            }
            this.showTable("select * from ChuyenNganh where DaXoa = 0 and TenCN like '%" + txtTimKiemTrash.Text + "%'");
        }

        private void txtTimKiemTrash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtTimKiemTrash.Text))
                {
                    this.messageDialog.Show("Vui lòng nhật tên để tìm kiếm.... ");
                    return;
                }
                this.showTable("select * from ChuyenNganh where DaXoa = 0 and TenCN like '%" + txtTimKiemTrash.Text + "%'");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = this.messageResult.Show();
            if (result == DialogResult.Yes)
            {
                txtTenCN.Text = "";
                cbAnHien.SelectedIndex = 0;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string TenCN = txtTenCN.Text.Trim();
            string trangThai = cbAnHien.SelectedItem.ToString();

            if (string.IsNullOrEmpty(TenCN) || string.IsNullOrEmpty(trangThai) || trangThai == "---- Chọn Trạng Thái ----")
            {
                this.messageDialog.Show("Vui lòng nhập đầy đủ các trường!");
                return;
            }

            ChuyenNganhDB chuyenNganh = new ChuyenNganhDB();
            bool isInsert = chuyenNganh.InsertData(TenCN, trangThai.Trim() != "Hiển Thị");
            this.messageDialog.Show(isInsert ? "Bạn đã thêm thành công!" : "Có lỗi xảy ra khi thêm!");
            txtTenCN.Text = "";
            cbAnHien.SelectedIndex = 0;
        }
    }
}

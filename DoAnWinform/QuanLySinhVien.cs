using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnWinform
{
    public partial class QuanLySinhVien : Form
    {
        public QuanLySinhVien()
        {
            InitializeComponent();
        }

        private void QuanLySinhVien_Load(object sender, EventArgs e)
        {

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
             DialogResult result  =this.messageResult.Show();
            if (result.Equals(DialogResult.OK)) {
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
    }
}

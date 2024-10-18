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

namespace DangKi_DangNhap
{

    public partial class login : Form
    {
        private string originalPassword = string.Empty;

        public login()
        {
            InitializeComponent();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {// Lấy giá trị người dùng nhập vào từ TextBox
         // Lấy giá trị người dùng nhập vào từ TextBox
            string username = txtUsername.Text;


        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Lấy ký tự cuối cùng mà người dùng vừa nhập
            string newChar = txtPassword.Text.Length > originalPassword.Length
                             ? txtPassword.Text.Substring(txtPassword.Text.Length - 1)
                             : string.Empty;

            // Cập nhật nội dung gốc của mật khẩu với ký tự mới
            originalPassword += newChar;

            // Đặt lại TextBox với ký tự *
            txtPassword.Text = new string('*', originalPassword.Length);

            // Đặt lại vị trí con trỏ ở cuối TextBox để không làm gián đoạn quá trình nhập liệu
            txtPassword.SelectionStart = txtPassword.Text.Length;
        }

        private void btn_Signup_Click(object sender, EventArgs e)
        {
            signup signup_form = new signup();
            this.Hide();
            signup_form.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}

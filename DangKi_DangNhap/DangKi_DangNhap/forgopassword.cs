using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DangKi_DangNhap
{
    public partial class forgopassword : Form
    {
        public forgopassword()
        {
            InitializeComponent();
        }
        private string confirmPassword = string.Empty;
        
        private void txtConfirmPass_TextChanged(object sender, EventArgs e)
        {
            // Lấy ký tự cuối cùng mà người dùng vừa nhập
            string newChar = txtRePass.Text.Length > confirmPassword.Length
                             ? txtRePass.Text.Substring(txtRePass.Text.Length - 1)
                             : string.Empty;

            // Cập nhật nội dung mật khẩu xác nhận
            confirmPassword += newChar;

            // Hiển thị ký tự * thay cho mật khẩu thực
            txtRePass.Text = new string('*', confirmPassword.Length);

            // Giữ con trỏ ở cuối TextBox
            txtRePass.SelectionStart = txtRePass.Text.Length;

        }
        private string newPassword = string.Empty;

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {
            // Lấy ký tự cuối cùng mà người dùng vừa nhập
            string newChar = txtNewPass.Text.Length > newPassword.Length
                             ? txtNewPass.Text.Substring(txtNewPass.Text.Length - 1)
                             : string.Empty;

            // Cập nhật nội dung mật khẩu mới
            newPassword += newChar;

            // Hiển thị ký tự * thay cho mật khẩu thực
            txtNewPass.Text = new string('*', newPassword.Length);

            // Giữ con trỏ ở cuối TextBox
            txtNewPass.SelectionStart = txtNewPass.Text.Length;

        }
    }
}

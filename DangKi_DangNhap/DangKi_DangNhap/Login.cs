using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;

namespace DangKi_DangNhap
{

    public partial class login : Form
    {
        private string originalPassword = string.Empty;

        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            LoadGifToPass();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            forgopassword forgotpass = new forgopassword();
            this.Hide();
            forgotpass.ShowDialog();
            this.Show();

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {// Lấy giá trị người dùng nhập vào từ TextBox
         // Lấy giá trị người dùng nhập vào từ TextBox
            string username = txtUsername.Text;


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
            // Firebase configuration
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };

            IFirebaseClient client = new FireSharp.FirebaseClient(config);

            if (client == null)
            {
                MessageBox.Show("Kết nối Firebase thất bại. Vui lòng kiểm tra lại cấu hình.", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = originalPassword; // Use the original password entered by the user

            // Retrieve user data from Firebase
            FirebaseResponse response = client.Get("Users/" + username);
            Users user = response.ResultAs<Users>(); // Deserialize the data to User object

            if (user == null)
            {
                MessageBox.Show("Không tìm thấy username.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the password matches using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (isPasswordValid)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    /*    public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }*/

        private void control_Close_Click(object sender, EventArgs e)
        {

        }

        private void control_Minimize_Click(object sender, EventArgs e)
        {

        }

        private bool pass_show = false;
        private void ptb_eye_new_pass_Click(object sender, EventArgs e)
        {
            pass_show = !pass_show;

            Task.Run(() =>
            {
                LoadGifToPass();
            });

            if (pass_show)
            {
                // Đặt PasswordChar là '\0' để bỏ ẩn ký tự, tức là hiển thị các ký tự thật
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';

            }
        }

        private void LoadGifToPass()
        {
            if (!pass_show)
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye_closed;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_new_pass.InvokeRequired)
                    {
                        ptb_eye_new_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_new_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_new_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_new_pass.InvokeRequired)
                    {
                        ptb_eye_new_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_new_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_new_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ptb_eye_pass_Click(object sender, EventArgs e)
        {

        }

        private void toggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch.Checked)
            {
                // Nếu công tắc bật, lưu tên người dùng và mật khẩu vào cài đặt
                Properties.Settings.Default.Username = txtUsername.Text;
                Properties.Settings.Default.Password = originalPassword; // Lưu mật khẩu gốc mà không bị che
                Properties.Settings.Default.RememberMe = true; // Đánh dấu Remember Me là true
                Properties.Settings.Default.Save(); // Lưu cài đặt
            }
            else
            {
                // Nếu công tắc tắt, xóa tên người dùng và mật khẩu đã lưu
                Properties.Settings.Default.Username = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
                Properties.Settings.Default.RememberMe = false; // Đánh dấu Remember Me là false
                Properties.Settings.Default.Save(); // Lưu cài đặt
            }
        }
    }
}

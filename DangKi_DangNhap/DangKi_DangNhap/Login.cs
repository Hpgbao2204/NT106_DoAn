using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;
using Quobject.SocketIoClientDotNet.Client;
using System.Net;



namespace DangKi_DangNhap
{
    public partial class login : Form
    {
        IFirebaseClient client;

        public login()
        {
            InitializeComponent();
            InitializeFirebaseClient();
        }

        private void InitializeFirebaseClient()
        {
            // Firebase configuration
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };

            client = new FireSharp.FirebaseClient(config);

            if (client == null)
            {
                MessageBox.Show("Kết nối Firebase thất bại. Vui lòng kiểm tra lại cấu hình.", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            LoadGifToPass();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            forgopassword forgotpass = new forgopassword();
            this.Hide();
            forgotpass.ShowDialog();
            this.Show();

        }

        private void btn_Signup_Click(object sender, EventArgs e)
        {
            signup signup_form = new signup();
            this.Hide();
            signup_form.ShowDialog();
            this.Show();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Kiểm tra nếu tên đăng nhập hoặc mật khẩu còn trống
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tên đăng nhập và mật khẩu.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thực hiện kết nối bất đồng bộ đến Firebase
                FirebaseResponse response = await client.GetAsync("Users/" + username);
                Users user = response.ResultAs<Users>();

                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy username.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the password matches using BCrypt
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (isPasswordValid)
                {
                    this.Hide();
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dashboard db = new dashboard(user);
                    db.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void toggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch.Checked)
            {
                // Nếu công tắc bật, lưu tên người dùng và mật khẩu vào cài đặt
                Properties.Settings.Default.Username = txtUsername.Text;
                //Properties.Settings.Default.Password = originalPassword; // Lưu mật khẩu gốc mà không bị che
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

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím Enter được nhấn
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi phương thức xử lý đăng nhập
                btnLogin_Click(sender, e);

                // Đánh dấu sự kiện là đã xử lý để ngăn các điều khiển khác xử lý lại
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}

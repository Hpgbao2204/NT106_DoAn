using FireSharp.Response;
using FireSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Interfaces;
using FireSharp.Config;
using System.Net;

namespace DangKi_DangNhap
{
    public partial class forgopassword : Form
    {
        private IFirebaseClient firebaseClient;
        public forgopassword()
        {
            InitializeComponent();

            lbl_request_status.Visible = false;
            lbl_status.Visible = false;

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };
            firebaseClient = new FireSharp.FirebaseClient(config);
        }

        private void control_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
        private string userEmail = string.Empty;
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            userEmail = txtEmail.Text;
            // Regular expression pattern to validate email format
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Check if the entered email matches the pattern
            if (System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                // If the email is valid, change the border color to green (or any visual indicator)
                txtEmail.BackColor = Color.LightGreen;
            }
            else
            {
                // If the email is not valid, change the border color to red (or any visual indicator)
                txtEmail.BackColor = Color.LightCoral;
            }
        }
        private string verificationCode;
        private async void btnVerify_Click(object sender, EventArgs e)
        {
            verificationCode = "";
            try
            {
                btnVerify.Enabled = false;
                btnVerify.Visible = false;

                // Kiểm tra email format
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Please enter an email address.", "Input Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!txtEmail.Text.EndsWith("@gm.uit.edu.vn"))
                {
                    MessageBox.Show("Email must end with '@gm.uit.edu.vn'.", "Input Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị thông báo đang gửi
                lbl_status.Text = "Sending verification code...";
                lbl_status.Visible = true;

                // Tạo mã xác nhận
                Random random = new Random();
                verificationCode = random.Next(1000, 9999).ToString();

                try
                {
                    await SendVerificationEmailAsync(txtEmail.Text, verificationCode);
                    // Chỉ hiển thị thông báo thành công khi thực sự gửi được
                    MessageBox.Show("The verification code has been sent to your email.",
                        "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SmtpFailedRecipientException ex) when (ex.Message.Contains("5.1.1"))
                {
                    // Mail không tồn tại
                    MessageBox.Show("The email address you entered appears to be invalid. Please check and try again.",
                        "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lbl_status.Text = "Invalid email address";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbl_status.Text = "Sending failed";
            }
            finally
            {
                btnVerify.Enabled = true;
                btnVerify.Visible = true;
                lbl_status.Visible = false;
            }
        }

        private async Task SendVerificationEmailAsync(string recipientEmailAddress, string verificationCode)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("thomasspielberg5@gmail.com"),
                Subject = "Your verification code",
                Body = $@"
                <html>
                    <body>
                        <h2>Verification Code</h2>
                            <p>Your verification code is: <strong>{verificationCode}</strong></p>
                            <p>If you didn't request this code, please ignore this email.</p>
                    </body>
                </html>",
                IsBodyHtml = true
            };
            mailMessage.To.Add(recipientEmailAddress);

            var smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("thomasspielberg5@gmail.com", "ekhs wtxd lvhk upfo"),
                EnableSsl = true
            };

            // Gửi email với timeout
            await smtpServer.SendMailAsync(mailMessage);
        }

        private void LoadGifToNewPass()
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

        private void LoadGifToRePass()
        {
            if (!confirm__pass_show)
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye_closed;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_re_pass.InvokeRequired)
                    {
                        ptb_eye_re_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_re_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_re_pass.Image = Image.FromStream(ms);
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
                    if (ptb_eye_re_pass.InvokeRequired)
                    {
                        ptb_eye_re_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_re_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_re_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool pass_show = false;
        private bool confirm__pass_show = false;
        private void ptb_eye_new_pass_Click(object sender, EventArgs e)
        {
            pass_show = !pass_show;
            Task.Run(() =>
            {
                LoadGifToNewPass();
            });

            if (pass_show)
            {
                // Đặt PasswordChar là '\0' để bỏ ẩn ký tự, tức là hiển thị các ký tự thật
                txtNewPass.PasswordChar = '\0';
            }
            else
            {
                txtNewPass.PasswordChar = '*';

            }
        }

        private void ptb_eye_confirm_re_pass_Click(object sender, EventArgs e)
        {
            confirm__pass_show = !confirm__pass_show;

            Task.Run(() =>
            {
                LoadGifToRePass();
            });

            if (confirm__pass_show)
            {
                // Đặt PasswordChar là '\0' để bỏ ẩn ký tự, tức là hiển thị các ký tự thật
                txtRePass.PasswordChar = '\0';
            }
            else
            {
                txtRePass.PasswordChar = '*';
            }
        }

        private void forgopassword_Load(object sender, EventArgs e)
        {
            // Khi form tải, đặt PasswordChar cho TextBox để hiện dấu hoa thị thay cho ký tự
            txtNewPass.PasswordChar= '*';
            txtRePass.PasswordChar = '*';

            Task.Run(() =>
            {
                LoadGifToNewPass();
            });

            Task.Run(() =>
            {
                LoadGifToRePass();
            });
        }

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string tentaikhoan = txtUsername.Text.Trim();
            string maXacThuc = txtCodeEmail.Text;
            string matKhau = txtNewPass.Text;
            string xacNhanMatKhau = txtRePass.Text;
            
            if (email == "" || tentaikhoan == "" || maXacThuc == "" || matKhau == "" || xacNhanMatKhau == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (maXacThuc != verificationCode)
            {
                MessageBox.Show("Mã xác thực không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (matKhau != xacNhanMatKhau)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp nhau!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {

                FirebaseResponse userResponse = await firebaseClient.GetAsync($"Users/{tentaikhoan}");
                if (userResponse.Body == "null")
                {
                    MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = userResponse.ResultAs<Users>();
                if (user.Email != (email))
                {
                    MessageBox.Show("Email không đúng hoặc không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnConfirm.Enabled = false;
                btnConfirm.Visible = false;

                lbl_request_status.Text = "Please wait a moment, we are processing your request...";
                lbl_request_status.Visible = true;

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);
                var Data = new Dictionary<string, object>
                    {
                    { "Password",  hashedPassword }
                    };
                FirebaseResponse response1 = await firebaseClient.UpdateAsync($"Users/{tentaikhoan}", Data);
                MessageBox.Show("Mật khẩu đã được đặt lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Text = "";
                txtNewPass.Text = "";
                txtRePass.Text = "";
                txtCodeEmail.Text = "";
                txtEmail.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lbl_request_status.Visible = false;

                btnConfirm.Enabled = true;
                btnConfirm.Visible = true;
            }
        }
    }
}
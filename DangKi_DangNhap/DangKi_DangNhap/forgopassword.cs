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

namespace DangKi_DangNhap
{
    public partial class forgopassword : Form
    {
        public forgopassword()
        {
            InitializeComponent();
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
                // Kiểm tra xem email có được nhập không
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Please enter an email address.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo mã xác nhận ngẫu nhiên gồm 4 chữ số
                Random random = new Random();
                verificationCode = random.Next(1000, 9999).ToString(); // Gán mã ngẫu nhiên cho biến cục bộ

                // Gọi phương thức gửi email bất đồng bộ
                await SendVerificationEmailAsync(txtEmail.Text, verificationCode);

                // Thông báo khi email được gửi thành công
                MessageBox.Show("The verification code has been sent to your email.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task SendVerificationEmailAsync(string emailAddress, string verificationCode)
        {
            try
            {
                // Cấu hình thông tin email để gửi
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("thomasspielberg5@gmail.com"); // Địa chỉ email của bạn
                mail.To.Add(emailAddress); // Địa chỉ email người nhận
                mail.Subject = "Your verification code";
                mail.Body = $"Your verification code is: {verificationCode}";

                // Sử dụng SmtpClient để gửi email
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // SMTP server của bạn, ví dụ Gmail
                smtpServer.Port = 587; // Cổng mặc định cho SMTP
                smtpServer.Credentials = new System.Net.NetworkCredential("thomasspielberg5@gmail.com", "ekhs wtxd lvhk upfo"); // Thông tin xác thực
                smtpServer.EnableSsl = true; // Kích hoạt SSL

                // Sử dụng async để gửi email bất đồng bộ
                await smtpServer.SendMailAsync(mail); // Gửi email
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while sending the email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}

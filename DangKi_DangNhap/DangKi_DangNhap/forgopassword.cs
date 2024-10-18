using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private string confirmPassword = string.Empty;
        
        private void txtConfirmPass_TextChanged(object sender, EventArgs e)
        {
            // Check if the user pressed the Backspace key
            if (Control.ModifierKeys == Keys.None && txtRePass.Text.Length < confirmPassword.Length)
            {
                // Remove the last character from the confirmPassword if Backspace is pressed
                confirmPassword = confirmPassword.Substring(0, confirmPassword.Length - 1);
            }
            else
            {
                // Otherwise, add the last entered character to confirmPassword
                string newChar = txtRePass.Text.Length > confirmPassword.Length
                                 ? txtRePass.Text.Substring(txtRePass.Text.Length - 1)
                                 : string.Empty;

                // Append the new character to the confirmPassword
                confirmPassword += newChar;
            }

            // Mask the TextBox input with asterisks
            txtRePass.Text = new string('*', confirmPassword.Length);

            // Set the cursor to the end of the TextBox
            txtRePass.SelectionStart = txtRePass.Text.Length;

        }
        private string newPassword = string.Empty;

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {
            // Check if the user pressed the Backspace key
            if (Control.ModifierKeys == Keys.None && txtNewPass.Text.Length < newPassword.Length)
            {
                // Remove the last character from newPassword if Backspace is pressed
                newPassword = newPassword.Substring(0, newPassword.Length - 1);
            }
            else
            {
                // Otherwise, add the last entered character to newPassword
                string newChar = txtNewPass.Text.Length > newPassword.Length
                                 ? txtNewPass.Text.Substring(txtNewPass.Text.Length - 1)
                                 : string.Empty;

                // Append the new character to newPassword
                newPassword += newChar;
            }

            // Mask the TextBox input with asterisks
            txtNewPass.Text = new string('*', newPassword.Length);

            // Set the cursor to the end of the TextBox
            txtNewPass.SelectionStart = txtNewPass.Text.Length;

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
    }
}

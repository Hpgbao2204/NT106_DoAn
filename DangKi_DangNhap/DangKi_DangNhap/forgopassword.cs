    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.IO;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using FireSharp;
    using FireSharp.Config;
    using FireSharp.Interfaces;
    using FireSharp.Response;
    using Microsoft.VisualBasic.ApplicationServices;

    namespace DangKi_DangNhap
    {
        public partial class forgopassword : Form
        {
            private IFirebaseClient firebaseClient;
            private bool isPasswordVisible = false;
            public forgopassword()
            {

                InitializeComponent();
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                    BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
                };
                firebaseClient = new FireSharp.FirebaseClient(config);
                errorLabel.Text = "";

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


        private void LoadGifToNewPass()
        {
            if (!new_pass_show)
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
            if (!re_pass_show)
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

        private bool new_pass_show = false;
        private bool re_pass_show = false;
        private void ptb_eye_new_pass_Click(object sender, EventArgs e)
        {
            new_pass_show = !new_pass_show;
            Task.Run(() =>
            {
                LoadGifToNewPass();
            });

            if (new_pass_show)
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
            re_pass_show = !re_pass_show;

            Task.Run(() =>
            {
                LoadGifToRePass();
            });

            if (re_pass_show)
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
            txtNewPass.PasswordChar = '*';
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
                errorLabel.ForeColor = Color.Red;// chỉ là đổi màu thành đỏ thôi
                //string encodedEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(email));
                if (email == "" || tentaikhoan == "" || maXacThuc == "" || matKhau == "" || xacNhanMatKhau == "")
                {
                    //MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorLabel.Text = "Vui lòng điền đầy đủ thông tin !";
                    return;
                }
                if (maXacThuc != verificationCode)
                {
                    errorLabel.Text = "Mã xác thực không đúng!";
                    return;
                }
                if (matKhau != xacNhanMatKhau)
                {
                    //MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp nhau!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorLabel.Text = "Xác nhận mật khẩu chưa đúng !";
                    return;
                }
                try
                {

                    FirebaseResponse userResponse = await firebaseClient.GetAsync($"Users/{tentaikhoan}");
                    if (userResponse.Body == "null")
                    {
                        //MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorLabel.Text = "Tài khoản không tồn tại !";
                        return;
                    }

                    var user = userResponse.ResultAs<Users>();
                    if (user.Email != (email))
                    {
                        //MessageBox.Show("Email không đúng vui lòng nhập lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorLabel.Text = "Email không đúng hoặc không tồn tại email này !";
                        return;
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);
                    var Data = new Dictionary<string, object>
                    {
                    { "MatKhau",  hashedPassword }
                    };
                    FirebaseResponse response1 = await firebaseClient.UpdateAsync($"Users/{tentaikhoan}", Data);
                    MessageBox.Show("Mật khẩu đã được đặt lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Text = "";
                    txtNewPass.Text = "";
                    txtRePass.Text = "";
                    txtCodeEmail.Text = "";
                    txtEmail.Text = "";
                    errorLabel.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void txtCodeEmail_TextChanged(object sender, EventArgs e)
            {

            }

            private void label1_Click(object sender, EventArgs e)
            {

            }
        }
    }
            
    

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;
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
using System.IO;


namespace DangKi_DangNhap
{
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
            BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"

        };

        IFirebaseClient client;

        private void signup_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(Config);
                if (client == null)
                {
                    MessageBox.Show("Can not connect to Server. Please re-check your configuration and Internet connection.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to connect to Firebase" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Khai báo biến cục bộ
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

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra các trường nhập liệu có được điền đầy đủ không
                if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtCreatePass.Text) ||
                    string.IsNullOrWhiteSpace(txtConfirmPass.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtCodeEmail.Text))
                {
                    MessageBox.Show("Please fill out all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra mã xác nhận người dùng nhập có khớp với mã đã gửi không
                if (txtCodeEmail.Text != verificationCode)
                {
                    MessageBox.Show("Verification code is incorrect. Please check your email and try again.", "Verification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu có khớp không
                if (txtCreatePass.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Password and confirmation password do not match.", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mã hóa mật khẩu bằng BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtCreatePass.Text);

                // Lấy ngày sinh từ DateTimePicker
                string birthday = date_time_picker.Value.ToString("yyyy-MM-dd"); // Chọn định dạng ngày tháng phù hợp

                // Kiểm tra xem tên đăng nhập đã tồn tại trong Firebase hay chưa
                FirebaseResponse checkResponse = await client.GetAsync("Users/" + txtUsername.Text);
                if (checkResponse == null)
                {
                    MessageBox.Show("Unable to retrieve data from Firebase.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Nếu tên đăng nhập đã tồn tại
                if (checkResponse.Body != "null")
                {
                    MessageBox.Show("Username already exists. Please choose a different username.", "Username Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClearInputFields();
                    return;
                }

                // Kiểm tra nếu người dùng đã chọn ảnh
                if (string.IsNullOrEmpty(selectedbase64ConvertedFromImage))
                {
                    MessageBox.Show("Please select an image to upload.", "Image Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng User để thêm vào Firebase
                var newUser = new Users
                {
                    Username = txtUsername.Text,
                    Password = hashedPassword, // Lưu mật khẩu đã mã hóa
                    Email = txtEmail.Text,
                    Birthday = birthday,
                    Image = selectedbase64ConvertedFromImage
                };

                // Gửi yêu cầu lên Firebase và nhận phản hồi
                SetResponse setResponse = await client.SetAsync("Users/" + txtUsername.Text, newUser);
                Users result = setResponse.ResultAs<Users>();

                // Thông báo khi người dùng mới đã được đăng ký thành công
                MessageBox.Show("User with username: " + result.Username + " has been successfully registered!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa các trường nhập liệu sau khi thành công
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            // Xóa nội dung các TextBox sau khi tạo tài khoản thành công
            txtUsername.Clear();
            txtCreatePass.Clear();
            txtConfirmPass.Clear();
            txtEmail.Clear();
            txtCodeEmail.Clear();
            selectedbase64ConvertedFromImage = "";
            verificationCode = "";
        }

        private string selectedbase64ConvertedFromImage;

        private void btnAvatar_Click(object sender, EventArgs e)
        {
            selectedbase64ConvertedFromImage = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Gọi phương thức để tải hình ảnh lên Firebase
                    string ImagePath = openFileDialog.FileName;
                    selectedbase64ConvertedFromImage = ConvertImageToBase64(ImagePath);
                    //SaveUserWithImage(base64Image);
                }
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
}

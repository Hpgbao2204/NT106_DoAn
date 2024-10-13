using System;
using System.IO;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using BCrypt.Net;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace DangKi_DangNhap
{
    public partial class TestDki : Form
    {
        IFirebaseClient firebaseClient;
        private string verificationCode;
        private const string Bucket = "gs://nt106-cce90.appspot.com";

        public TestDki()
        {
            InitializeComponent();

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };
            firebaseClient = new FireSharp.FirebaseClient(config);
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;
            string username = usernameTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                errorLabel.Text = "Vui lòng điền đầy đủ thông tin!";
                return;
            }

            string encodedEmail = EncodeEmail(email);

            // Kiểm tra email đã tồn tại chưa
            FirebaseResponse emailResponse = await firebaseClient.GetAsync($"emails/{encodedEmail}");
            if (emailResponse.Body != "null")
            {
                errorLabel.Text = "Email đã tồn tại!";
                return;
            }

            // Mã hóa mật khẩu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Tạo người dùng mới
            var newUser = new User
            {
                Email = email,
                Password = hashedPassword,
                Username = username
            };

            // Lưu người dùng vào Firebase
            SetResponse response = await firebaseClient.SetAsync($"users/{username}", newUser);
            await firebaseClient.SetAsync($"emails/{encodedEmail}", true);
            MessageBox.Show("Đăng ký thành công!");

            ClearForm();
        }

        // Gửi mã xác thực qua email
        private void SendVerificationCode(string email)
        {
            verificationCode = GenerateVerificationCode();
            string fromAddress = "your-email@gmail.com";
            string subject = "Verification Code";
            string body = $"Mã xác thực của bạn là: {verificationCode}";

            using (MailMessage mail = new MailMessage(fromAddress, email, subject, body))
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress, "your-email-password")
                };
                smtp.Send(mail);
            }
        }

        // Tạo mã xác thực
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Mã hóa email để tránh ký tự không hợp lệ
        private string EncodeEmail(string email)
        {
            return email.Replace(".", "-dot-").Replace("@", "-at-");
        }

        // Xóa các trường sau khi đăng ký
        private void ClearForm()
        {
            emailTextBox.Text = "";
            passwordTextBox.Text = "";
            usernameTextBox.Text = "";
            errorLabel.Text = "";
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}

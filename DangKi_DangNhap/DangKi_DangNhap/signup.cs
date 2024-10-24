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
using FireSharp;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;


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
                    if (ptb_eye_pass.InvokeRequired)
                    {
                        ptb_eye_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_pass.InvokeRequired)
                    {
                        ptb_eye_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadGifToConfirmPass()
        {
            if (!confirm__pass_show)
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye_closed;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_confirm_pass.InvokeRequired)
                    {
                        ptb_eye_confirm_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_confirm_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_confirm_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                try
                {
                    byte[] gifData = Properties.Resources.eye;

                    // Tạo MemoryStream từ byte array
                    MemoryStream ms = new MemoryStream(gifData);

                    // Kiểm tra nếu cần Invoke
                    if (ptb_eye_confirm_pass.InvokeRequired)
                    {
                        ptb_eye_confirm_pass.Invoke(new MethodInvoker(delegate
                        {
                            ptb_eye_confirm_pass.Image = Image.FromStream(ms);
                        }));
                    }
                    else
                    {
                        ptb_eye_confirm_pass.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void signup_Load(object sender, EventArgs e)
        {
            // Giấu đi thành phần picture box (hiển thị avatar của người dùng) cho tới khi người dùng chọn ảnh thì hiện lên cùng với ảnh mà người dùng chọn
            ptb_avatar.Hide();

            // Ẩn đi dòng hiển thị trạng gửi mail có mã xác nhận
            lbl_status.Visible = false;

            // Ẩn đi dòng hiển thị trạng gửi mail có mã xác nhận
            lbl_create_acc_status.Visible = false;

            // Khi form tải, đặt PasswordChar cho TextBox để hiện dấu hoa thị thay cho ký tự
            txtCreatePass.PasswordChar = '*';
            txtConfirmPass.PasswordChar = '*';

            Task.Run(() =>
            {
                LoadGifToPass();
            });

            Task.Run(() =>
            {
                LoadGifToConfirmPass();
            });

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
                btnVerify.Enabled = false;
                btnVerify.Visible = false;

                // Kiểm tra xem đã nhập trường Email hay chưa
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
                    //lbl_status.Text = "Code sent successfully!";
                }
                catch (SmtpFailedRecipientException ex) when (ex.Message.Contains("5.1.1")) //Mã lỗi "5.1.1" là một mã trạng thái SMTP có nghĩa là
                                                                                            //"Recipient address rejected: User unknown in virtual mailbox table".
                                                                                            //Điều này thường xảy ra khi địa chỉ email người nhận không hợp lệ hoặc
                                                                                            //không tồn tại.
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

        public class AccountValidator
        {
            // Định nghĩa các mã lỗi validation
            public enum ValidationError
            {
                None,
                EmptyFields,
                InvalidVerificationCode,
                PasswordMismatch,
                MissingProfilePicture,
                InvalidImage,
                UsernameTaken,
                PasswordTooShort
            }

            // Class chứa kết quả validation
            public class ValidationResult
            {
                public bool IsValid { get; set; }
                public ValidationError Error { get; set; }
                public string Message { get; set; }
            }

            private readonly IFirebaseClient _client;

            public AccountValidator(IFirebaseClient client)
            {
                _client = client;
            }

            // Phương thức validation chính
            public async Task<ValidationResult> ValidateAccountCreation(
                string username,
                string password,
                string confirmPassword,
                string email,
                string verificationCode,
                string actualVerificationCode,
                string base64Image,
                bool hasChosenPicture)
            {
                // Sử dụng cache để tránh validate lại các giá trị không đổi
                var cachedValidations = new ConcurrentDictionary<string, bool>();

                // Kiểm tra fields trống
                if (!cachedValidations.GetOrAdd("fieldsNotEmpty", _ =>
                    !string.IsNullOrWhiteSpace(username) &&
                    !string.IsNullOrWhiteSpace(password) &&
                    !string.IsNullOrWhiteSpace(confirmPassword) &&
                    !string.IsNullOrWhiteSpace(email) &&
                    !string.IsNullOrWhiteSpace(verificationCode)))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Error = ValidationError.EmptyFields,
                        Message = "Please fill out all the fields."
                    };
                }

                // Kiểm tra mã xác thực
                if (verificationCode != actualVerificationCode)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Error = ValidationError.InvalidVerificationCode,
                        Message = "Verification code is incorrect."
                    };
                }

                // Kiểm tra password match
                if (!cachedValidations.GetOrAdd("passwordsMatch", _ =>
                    password == confirmPassword))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Error = ValidationError.PasswordMismatch,
                        Message = "Passwords do not match."
                    };
                }

                // Kiểm tra mật khẩu có ít nhất 8 ký tự
                if (!cachedValidations.GetOrAdd("passwordLength", _ =>
                    password.Length >= 8))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Error = ValidationError.PasswordTooShort,
                        Message = "Password must be at least 8 characters long."
                    };
                }

                // Kiểm tra ảnh đại diện
                if (!hasChosenPicture || string.IsNullOrEmpty(base64Image))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Error = ValidationError.MissingProfilePicture,
                        Message = "Please choose a profile picture."
                    };
                }

                // Kiểm tra username tồn tại - thao tác async duy nhất
                try
                {
                    var checkResponse = await _client.GetAsync($"Users/{username}");
                    if (checkResponse?.Body != "null")
                    {
                        return new ValidationResult
                        {
                            IsValid = false,
                            Error = ValidationError.UsernameTaken,
                            Message = "Username already exists."
                        };
                    }
                }
                catch
                {
                    throw new Exception("Unable to verify username availability.");
                }

                return new ValidationResult
                {
                    IsValid = true,
                    Error = ValidationError.None
                };
            }
        }

            // Sử dụng trong button click event
        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                btnCreateAccount.Enabled = false; // vô hiệu nút này để tránh việc người dùng spam

                var validator = new AccountValidator(client);
                var validationResult = await validator.ValidateAccountCreation(
                    txtUsername.Text,
                    txtCreatePass.Text,
                    txtConfirmPass.Text,
                    txtEmail.Text,
                    txtCodeEmail.Text,
                    verificationCode,
                    selectedbase64ConvertedFromImage,
                    choosePicture
                );

                if (!validationResult.IsValid)
                {
                    MessageBox.Show(validationResult.Message, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tiến hành tạo tài khoản khi validation passed
                btnCreateAccount.Visible = false; // ẩn nút tạo tài khoản để hiện lên trạng thái

                lbl_create_acc_status.Text = "Please wait a moment, we are processing your request...";
                lbl_create_acc_status.Visible = true;

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtCreatePass.Text);
                var birthday = date_time_picker.Value.ToString("yyyy-MM-dd");

                var newUser = new Users
                {
                    Username = txtUsername.Text,
                    Password = hashedPassword,
                    Email = txtEmail.Text,
                    Birthday = birthday,
                    Image = selectedbase64ConvertedFromImage
                };

                var setResponse = await client.SetAsync($"Users/{txtUsername.Text}", newUser);
                var result = setResponse.ResultAs<Users>();

                MessageBox.Show($"User with username: {result.Username} has been successfully registered!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // kích hoạt và hiện lại nút tạo tài khoản
                btnCreateAccount.Enabled = true;
                btnCreateAccount.Visible = true;

                // Khi tạo xong rồi thì ẩn đi dòng trạng thái
                lbl_create_acc_status.Visible = false;
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

        private bool choosePicture = false;
        private void btnAvatar_Click(object sender, EventArgs e)
        {
            selectedbase64ConvertedFromImage = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    choosePicture = true;
                    // Gọi phương thức để tải hình ảnh lên Firebase
                    string ImagePath = openFileDialog.FileName;
                    selectedbase64ConvertedFromImage = ConvertImageToBase64(ImagePath);
                    //SaveUserWithImage(base64Image);

                    label_avatar.Hide();
                    ptb_avatar.Image = Image.FromFile(ImagePath);
                    ptb_avatar.Show();
                }
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private bool pass_show = false;
        private bool confirm__pass_show = false;
        private void ptb_eye_pass_Click(object sender, EventArgs e)
        {
            pass_show = !pass_show;
            Task.Run(() =>
            {
                LoadGifToPass();
            });

            if (pass_show)
            {
                // Đặt PasswordChar là '\0' để bỏ ẩn ký tự, tức là hiển thị các ký tự thật
                txtCreatePass.PasswordChar = '\0';
            } else
            {
                txtCreatePass.PasswordChar = '*';

            }
        }

        private void ptb_eye_confirm_pass_Click(object sender, EventArgs e)
        {
            confirm__pass_show = !confirm__pass_show;

            Task.Run(() =>
            {
                LoadGifToConfirmPass();
            });

            if (confirm__pass_show)
            {
                // Đặt PasswordChar là '\0' để bỏ ẩn ký tự, tức là hiển thị các ký tự thật
                txtConfirmPass.PasswordChar = '\0';
            }
            else
            {
                txtConfirmPass.PasswordChar = '*';
            }
        }
    }
}

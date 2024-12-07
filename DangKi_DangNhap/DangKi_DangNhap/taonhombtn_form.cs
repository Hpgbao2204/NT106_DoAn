using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;

namespace DangKi_DangNhap
{
    public partial class taonhombtn_form : Form
    {
        private IFirebaseConfig Config;
        private IFirebaseClient client;
        private Users _currentUser;
        private SocketIOClient.SocketIO _clientSocket;

        public taonhombtn_form(Users currentUser, SocketIOClient.SocketIO socket)
        {
            InitializeComponent();
            InitializeFirebase();

            _currentUser = currentUser;
            _clientSocket = socket;
        }

        private void InitializeFirebase()
        {
            Config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };

            client = new FirebaseClient(Config); // Khởi tạo client Firebase

            // Initialize Firebase Admin SDK
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("F:/Onedrive/OneDrive/UIT Lecture/Lap Trinh Mang/Do an cua minh/NT106_DoAn/DangKi_DangNhap/DangKi_DangNhap/bin/Debug/serviceAccountKey.json"),
            });
        }

        private async Task<string> GetAnonymousUserTokenAsync()
        {
            // Authenticate the user anonymously
            var auth = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
            {
                DisplayName = "AnonymousUser",
            });

            var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(auth.Uid);
            return token;
        }

        private async void btnTaoNhom_Click(object sender, EventArgs e)
        {
            // Lấy ID phòng từ TextBox
            string groupId = txtID.Text.Trim();
            string groupName = txtTenNhom.Text;
            string userName = txtUsername.Text;

            // Kiểm tra nếu Id hoặc tên nhóm rỗng
            if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ ID và Tên của nhóm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra xem mã phòng đã tồn tại trong database chưa
                FirebaseResponse checkResponse = await client.GetAsync("Rooms/" + groupId);
                if (checkResponse == null)
                {
                    MessageBox.Show("Không thể lấy dữ liệu từ Firebase.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tiếp tục kiểm tra nếu tên người dùng đã tồn tại
                if (checkResponse.Body != null && checkResponse.Body != "null")
                {
                    MessageBox.Show("Mã phòng đã tồn tại. Vui lòng chọn mã phòng khác.", "Mã phòng trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await _clientSocket.EmitAsync("new-room-created", new
                {
                    roomId = groupId,
                    roomName = groupName,
                    creator = userName,
                    members = new List<string> { userName } // Thêm creator vào danh sách members
                });

                // Tạo folder trong Firebase Storage cho nhóm
                var token = await GetAnonymousUserTokenAsync();
                await CreateStorageFolderForGroup(groupId, token);

                MessageBox.Show($"Đã tạo phòng với ID: {groupId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Đóng form sau khi tạo phòng
                this.Close();
            }
            catch (Exception ex)
            {
                // Thông báo lỗi khi không thể thêm phòng vào Firebase
                MessageBox.Show($"Lỗi khi tạo phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CreateStorageFolderForGroup(string groupId, string authToken)
        {
            string bucketName = "nt106-cce90.appspot.com"; // Thay bằng tên bucket của bạn
            string filePath = $"groups/{groupId}/dummy.txt"; // Tạo file dummy trong thư mục mới
            string uploadUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o?name={filePath}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                    // Nội dung file dummy
                    var content = new StringContent("This is a dummy file for folder creation.");
                    content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                    // Gửi yêu cầu HTTP POST để tạo file
                    var response = await client.PostAsync(uploadUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Created folder-like structure 'groups/{groupId}' in bucket {bucketName}.");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to create folder: {response.StatusCode}, {response.ReasonPhrase}, {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex.Message}");
                throw;
            }
        }


        private void bt_esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

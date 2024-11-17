﻿using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Response;
using System.IO;
using SocketIOClient; // Đảm bảo bạn đã thêm dòng này
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DangKi_DangNhap
{
    public partial class NhomHoc_form : Form
    {
        private Socket socket;
        private IFirebaseConfig Config;
        private IFirebaseClient client;
        private Users _currentUser;
        private SocketIOClient.SocketIO clientSocket; // Đây là khai báo 

        public NhomHoc_form(Users currentUser)
        {
            InitializeComponent();
            InitializeSocketIO();
            InitializeFirebase();
            _currentUser = currentUser;
        }

        private void InitializeFirebase()
        {
            Config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };

            client = new FirebaseClient(Config); // Khởi tạo client Firebase
        }

        private void InitializeSocketIO()
        {
            clientSocket = new SocketIOClient.SocketIO("http://localhost:3000");
            clientSocket.OnConnected += async (sender, e) =>
            {
                MessageBox.Show("Connected to Socket.IO server");
            };
            clientSocket.ConnectAsync();
        }

        private async Task LoadRooms()
        {
            try
            {
                clientSocket.On("roomsData", response =>
                {
                    try
                    {
                        MessageBox.Show("Đã nhận được danh sách phòng từ server.");

                        // Thay đổi cách lấy và deserialize dữ liệu
                        var roomsDictionary = response.GetValue<Dictionary<string, RoomDetail>>();

                        if (roomsDictionary == null || roomsDictionary.Count == 0)
                        {
                            MessageBox.Show("Không có phòng nào trong cơ sở dữ liệu.");
                            return;
                        }

                        // Đảm bảo cập nhật UI trên thread chính
                        if (flowLayoutPanel1.InvokeRequired)
                        {
                            flowLayoutPanel1.Invoke(new Action(() =>
                            {
                                UpdateRoomsList(roomsDictionary);
                            }));
                        }
                        else
                        {
                            UpdateRoomsList(roomsDictionary);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xử lý danh sách phòng: {ex.Message}");
                    }
                });

                await clientSocket.ConnectAsync();
                await clientSocket.EmitAsync("getRooms");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kết nối hoặc xử lý sự kiện: {ex.Message}");
            }
        }

        // Tách riêng phần cập nhật UI
        private void UpdateRoomsList(Dictionary<string, RoomDetail> roomsDictionary)
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var room in roomsDictionary)
            {
                var roomLabel = new Label
                {
                    Text = $"{room.Value.RoomId} - {room.Value.RoomName}",
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true
                };
                flowLayoutPanel1.Controls.Add(roomLabel);
            }
        }

        // Đảm bảo class RoomDetail có đúng các properties
        public class RoomDetail
        {
            public long CreatedAt { get; set; }
            public string Creator { get; set; }
            public string RoomId { get; set; }
            public string RoomName { get; set; }
        }


        private async void loadBasicInfo()
        {
            try
            {
                // Load tên người dùng vào label txtName
                txtName.Text = _currentUser.Username;

                // Truy xuất ảnh base64 từ Firebase
                FirebaseResponse response = await client.GetAsync($"Users/{_currentUser.Username}/Image");
                string imageBase64 = response.ResultAs<string>();

                // Kiểm tra nếu chuỗi base64 không rỗng hoặc null
                if (!string.IsNullOrEmpty(imageBase64))
                {
                    // Giải mã base64 thành byte array
                    byte[] imageBytes = Convert.FromBase64String(imageBase64);

                    // Tạo MemoryStream từ byte array
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        // Chuyển đổi MemoryStream thành Image và hiển thị trên PictureBox
                        pbAvatar.Image = Image.FromStream(ms);
                        pbAvatar.Visible = true; // Hiển thị ảnh sau khi tải thành công
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ảnh trong cơ sở dữ liệu Firebase.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối Firebase: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void NhomHoc_form_Load(object sender, EventArgs e)
        {
            pbAvatar.Visible = false;
            loadBasicInfo();
            await LoadRooms();
        }
        private void btnTaoNhom_Click(object sender, EventArgs e)
        {
            taonhombtn_form TaoNhom = new taonhombtn_form(_currentUser, clientSocket);
            TaoNhom.ShowDialog();
        }
        private void btnThamGia_Click(object sender, EventArgs e)
        {
            thamgiabtn_form ThamGiaNhom = new thamgiabtn_form(_currentUser, clientSocket);
            ThamGiaNhom.ShowDialog();
        }
        private void btnNhomHoc_Click(object sender, EventArgs e)
        {
            // Gọi lại phương thức load form để tải lại dữ liệu
            NhomHoc_form_Load(sender, e);
        }
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            this.Hide();

            dashboard TrangChu = new dashboard(_currentUser);
            TrangChu.Show();
        }
    }
}

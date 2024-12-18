
using FireSharp.Config;
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
using SocketIOClient;
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
                MessageBox.Show("Vui lòng đẩy đủ nhập ID và Tên của nhóm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra xem mã phòng đã tồn tại trong database chưa
                FirebaseResponse checkResponse = await client.GetAsync("Rooms/" + txtID.Text);
                if (checkResponse == null)
                {
                    MessageBox.Show("Không thể lấy dữ liệu từ Firebase.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tiếp tục kiểm tra nếu tên người dùng đã tồn tại
                if (checkResponse.Body != "null")
                {
                    MessageBox.Show("Mã phòng đã tồn tại. Vui lòng chọn mã phòng khác khác.", "Mã phòng trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await _clientSocket.EmitAsync("new-room-created", new
                {
                    roomId = groupId,
                    roomName = groupName,
                    creator = userName,
                    members = new List<string> { userName } // Thêm creator vào danh sách members
                });

                MessageBox.Show($"Đã tạo phòng với ID: {groupId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally, you could trigger an event here to notify other users via Socket.io about the new room


                // Close the form after creating the room
                this.Close();
            }
            catch (Exception ex)
            {
                // Thông báo lỗi khi không thể thêm phòng vào Firebase
                MessageBox.Show($"Lỗi khi tạo phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void controlboxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

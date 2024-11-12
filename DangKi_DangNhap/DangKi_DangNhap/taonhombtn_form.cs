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

namespace DangKi_DangNhap
{
    public partial class taonhombtn_form : Form
    {
        private Users _currentUser;
        public taonhombtn_form(Users currentUser)
        {
            InitializeComponent();
            InitializeFirebase();

            _currentUser = currentUser;
        }

        IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
            BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"

        };

        IFirebaseClient client;

        private void InitializeFirebase()
        {
            try
            {
                client = new FirebaseClient(Config);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to Firebase: " + ex.Message);
            }
        }

        private async void btnTaoNhom_Click(object sender, EventArgs e)
        {
            // Lấy ID phòng từ TextBox
            string groupId = txtID.Text.Trim();
            string groupName = txtTenNhom.Text;

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

                // Chuẩn bị dữ liệu phòng mới
                var roomData = new RoomDetail
                {
                    RoomId = groupId,
                    RoomName = groupName,
                    CreatorName = _currentUser.Username,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    Members = _currentUser.Username // Người tạo sẽ là thành viên đầu tiên
                };

                // Đưa dữ liệu phòng lên Firebase
                // Gửi yêu cầu lên Firebase và nhận phản hồi
                SetResponse setResponse = await client.SetAsync("Rooms/" + txtID.Text, roomData);
                RoomDetail result = setResponse.ResultAs<RoomDetail>();

                MessageBox.Show($"Đã tạo phòng với ID: {groupId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}

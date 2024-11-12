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
    public partial class thamgiabtn_form : Form
    {
        private Users _currentUser;

        public thamgiabtn_form(Users currentUser)
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

        private void bt_esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            cmbNhom.Items.Clear();
            txtIDNhom.Clear();

            try
            {
                string currUsername = txtTenNguoiTao.Text;
                if (string.IsNullOrEmpty(currUsername))
                {
                    MessageBox.Show("Vui lòng nhập tên người tạo", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy tất cả các nhóm từ Firebase
                FirebaseResponse response = await client.GetAsync("Rooms");

                if (response == null || response.Body == "null")
                {
                    MessageBox.Show("Không có phòng nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy dữ liệu từ phản hồi Firebase
                Dictionary<string, RoomDetail> allRooms = response.ResultAs<Dictionary<string, RoomDetail>>();

                // Lọc các phòng do người dùng hiện tại tạo
                var userRooms = allRooms
                    .Where(room => room.Value.CreatorName == currUsername)
                    .Select(room => new { ID = room.Key, Name = room.Value.RoomName })
                    .ToList();

                // Kiểm tra nếu người dùng không có phòng nào
                if (userRooms.Count == 0)
                {
                    MessageBox.Show("Người dùng này không có phòng nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Xóa các mục hiện tại trong ComboBox và thêm các phòng tìm được
                cmbNhom.Items.Clear();
                foreach (var room in userRooms)
                {
                    cmbNhom.Items.Add($"{room.ID} - {room.Name}");
                }

                // Hiển thị mục đầu tiên ngay khi có kết quả
                if (cmbNhom.Items.Count > 0)
                {
                    cmbNhom.SelectedIndex = 0;
                }

                MessageBox.Show("Đã tìm thấy các phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNhom.SelectedItem != null)
            {
                // Lấy chuỗi của mục đang chọn, định dạng là "ID - Tên phòng"
                string selectedRoom = cmbNhom.SelectedItem.ToString();

                // Tách chuỗi để lấy phần ID (phần trước dấu '-')
                string roomId = selectedRoom.Split('-')[0].Trim();

                // Hiển thị ID trong txtID
                txtIDNhom.Text = roomId;
            }
        }

        private void txtTenNguoiTao_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím Enter được nhấn
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi phương thức xử lý đăng nhập
                btnTimKiem_Click(sender, e);

                // Đánh dấu sự kiện là đã xử lý để ngăn các điều khiển khác xử lý lại
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private async void btnThamGiaNhom_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy ID phòng từ textbox
                string roomId = txtIDNhom.Text.Trim();

                // Kiểm tra nếu ID phòng rỗng
                if (string.IsNullOrEmpty(roomId))
                {
                    MessageBox.Show("Vui lòng nhập ID phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy phòng từ Firebase
                FirebaseResponse response = await client.GetAsync($"Rooms/{roomId}");

                if (string.IsNullOrEmpty(response.Body) || response.Body == "null")
                {
                    MessageBox.Show("Không tìm thấy phòng với ID đã nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin phòng từ Firebase
                RoomDetail room = response.ResultAs<RoomDetail>();

                // Kiểm tra xem người dùng đã là thành viên của phòng chưa
                if (!string.IsNullOrEmpty(room.Members))
                {
                    var members = room.Members.Split(',');
                    if (members.Contains(_currentUser.Username))
                    {
                        MessageBox.Show("Bạn đã tham gia nhóm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();

                        chatnhom GroupChat = new chatnhom();
                        GroupChat.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Bạn chưa được người tạo nhóm thêm vào danh sách thành viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Người dùng chưa được thêm vào nhóm
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tham gia nhóm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

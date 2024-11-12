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
using System.IO;

namespace DangKi_DangNhap
{
    public partial class NhomHoc_form : Form
    {
        private Users _currentUser;

        public NhomHoc_form(Users currentUser)
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

        private void btnTaoNhom_Click(object sender, EventArgs e)
        {
            taonhombtn_form TaoNhom = new taonhombtn_form(_currentUser);
            TaoNhom.ShowDialog();
        }

        private void btnThamGia_Click(object sender, EventArgs e)
        {
            thamgiabtn_form ThamGiaNhom = new thamgiabtn_form(_currentUser);
            ThamGiaNhom.ShowDialog();
        }

        private async void NhomHoc_form_Load(object sender, EventArgs e)
        {
            // Đặt pbAvatar không hiển thị cho đến khi ảnh được tải về
            pbAvatar.Visible = false;

            loadBasicInfo();

            try
            {
                // Lấy tất cả các nhóm từ Firebase
                FirebaseResponse response = await client.GetAsync("Rooms");

                if (response == null || response.Body == "null")
                {
                    MessageBox.Show("Không có phòng nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy dữ liệu từ phản hồi Firebase
                Dictionary<string, RoomDetail> allRooms = response.ResultAs<Dictionary<string, RoomDetail>>();

                // Xóa tất cả các điều khiển hiện tại trong FlowLayoutPanel để làm mới
                flowLayoutPanel1.Controls.Clear();

                // Thiết lập FlowLayoutPanel để sắp xếp theo chiều dọc
                flowLayoutPanel1.FlowDirection = FlowDirection.TopDown; // Sắp xếp theo chiều dọc
                flowLayoutPanel1.WrapContents = false; // Không cho phép xuống hàng khi không còn không gian

                // Lọc và lấy tối đa 10 phòng đầu tiên
                var limitedRooms = allRooms.Take(10).ToList();

                // Duyệt qua tất cả các phòng và thêm vào FlowLayoutPanel
                foreach (var room in limitedRooms)
                {
                    // Tạo Label để hiển thị ID và tên phòng
                    Label roomLabel = new Label
                    {
                        Text = $"{room.Value.RoomId} - {room.Value.RoomName}",
                        Font = new Font("Segoe UI", 10), // Font Segoe UI, kiểu thường
                        AutoSize = true,
                        Padding = new Padding(10),
                        Margin = new Padding(5),
                        TextAlign = ContentAlignment.MiddleLeft
                    };

                    // Thêm Label vào FlowLayoutPanel
                    flowLayoutPanel1.Controls.Add(roomLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

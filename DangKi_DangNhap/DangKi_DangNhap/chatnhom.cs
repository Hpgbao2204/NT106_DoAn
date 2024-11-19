using SocketIOClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DangKi_DangNhap
{
    public partial class chatnhom : Form
    {
        private Users _currentUser;
        private SocketIOClient.SocketIO _clientSocket;
        private string _roomID;
        public chatnhom(string roomID, Users User, SocketIOClient.SocketIO socket)
        {
            InitializeComponent();

            _currentUser = User;
            _clientSocket = socket;
            _roomID = roomID;
        }

        private async void chatnhom_Load(object sender, EventArgs e)
        {
            try
            {
                // Yêu cầu server gửi lại danh sách thành viên của nhóm
                _clientSocket.EmitAsync("get-members", _roomID);

                // Lắng nghe sự kiện "members-list" từ server
                _clientSocket.On("members-list", (SocketIOResponse members) =>
                {
                    // Lấy danh sách thành viên từ dữ liệu trả về
                    var membersList = members.GetValue<List<string>>();

                    // Cập nhật giao diện với danh sách thành viên
                    if (membersList != null && membersList.Count > 0)
                    {
                        groupboxDs.Invoke(new Action(() =>
                        {
                            // Clear các Label cũ nếu có
                            groupboxDs.Controls.Clear();

                            // Tạo các Label mới cho mỗi thành viên
                            int yPosition = 50;  // Vị trí bắt đầu cho Label
                            foreach (var member in membersList)
                            {
                                Label memberLabel = new Label
                                {
                                    Text = member,
                                    Location = new Point(10, yPosition),
                                    AutoSize = true
                                };
                                groupboxDs.Controls.Add(memberLabel);
                                yPosition += 30;  // Dịch chuyển xuống dưới cho Label kế tiếp
                            }
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Không có thành viên nào trong nhóm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                });

                // Lắng nghe sự kiện lỗi
                _clientSocket.On("error", (errorMessage) =>
                {
                    MessageBox.Show(errorMessage.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }
    }
}

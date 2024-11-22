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

                await _clientSocket.EmitAsync("get-messages", _roomID);

                // Lắng nghe sự kiện tải lịch sử tin nhắn 
                _clientSocket.On("message-history", (SocketIOResponse response) =>
                {
                    // Lấy danh sách tin nhắn từ server
                    var historyList = response.GetValue<List<ChatMessage>>();

                    // Hiển thị lịch sử tin nhắn
                    if (historyList != null && historyList.Count > 0)
                    {
                        rtbChat.Invoke(new Action(() =>
                        {
                            rtbChat.Clear();
                            foreach (var message in historyList)
                            {
                                rtbChat.AppendText($"{message.Sender}: {message.Content}\n");
                            }
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Không có tin nhắn nào trong nhóm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                });

                // Lắng nghe tin nhắn mới
                

                await _clientSocket.EmitAsync("join-room", _roomID);

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

        private void rtbChat_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnDang_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nội dung tin nhắn
                string messageContent = txtChatbox.Text.Trim();
                if (string.IsNullOrEmpty(messageContent))
                {
                    MessageBox.Show("Vui lòng nhập nội dung tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng tin nhắn
                var newMessage = new
                {
                    RoomID = _roomID,
                    Sender = _currentUser.Username, // Lấy tên người dùng hiện tại
                    Content = messageContent,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() // Thời gian gửi tin nhắn
                };

                if (string.IsNullOrEmpty(_roomID) || string.IsNullOrEmpty(_currentUser.Username))
                {
                    MessageBox.Show("Lỗi: Không thể gửi tin nhắn vì thiếu thông tin phòng hoặc người gửi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine($"Gửi tin nhắn: {System.Text.Json.JsonSerializer.Serialize(newMessage)}");

                // Gửi tin nhắn đến server
                await _clientSocket.EmitAsync("send-message", newMessage);

                _clientSocket.On("new-message", (SocketIOResponse response) =>
                {
                    var newMessage_respone = response.GetValue<ChatMessage>();

                    if (newMessage_respone != null)
                    {
                        rtbChat.Invoke(new Action(() =>
                        {
                            rtbChat.AppendText($"{newMessage_respone.Sender}: {newMessage_respone.Content}\n");
                        }));
                    }
                });

                // Xóa khung nhập sau khi gửi
                txtChatbox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

public class ChatMessage
{
    public string Id { get; set; }
    public string Sender { get; set; }
    public string Content { get; set; }
    public long Timestamp { get; set; }
}
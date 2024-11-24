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
using System.Text.Json;
using System.Text.Json.Serialization;

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
                // Gửi yêu cầu nhận danh sách thành viên
                await _clientSocket.EmitAsync("get-members", _roomID);

                // Lắng nghe danh sách thành viên
                _clientSocket.On("members-list", (SocketIOResponse response) =>
                {
                    var membersList = response.GetValue<List<string>>();
                    UpdateMemberListUI(membersList);
                });

                // Tải lịch sử tin nhắn
                await LoadChatHistory();

                _clientSocket.On("new-message", (SocketIOResponse response) =>
                {
                    try
                    {
                        Console.WriteLine("Đã nhận được sự kiện 'new-message' từ server!");
                        string raw = response.ToString();
                        Console.WriteLine($"JSON thô: {raw}");

                        // Dùng dynamic để nhận dữ liệu không xác định trước
                        var newMessage = response.GetValue<NewMessage>();
                        Console.WriteLine("New message received:");

                        // Kiểm tra RoomID trước khi xử lý tin nhắn
                        string roomId = newMessage.RoomID?.ToString(); // Chuyển RoomID sang string
                        if (roomId != null && roomId == _roomID) // Kiểm tra RoomID
                        {
                            // Tạo một đối tượng ChatMessage từ dynamic, chỉ lấy các thuộc tính cần thiết
                            var message = new ChatMessage
                            {
                                Content = newMessage.Content,
                                Sender = newMessage.Sender,
                                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                            };

                            // Hiển thị tin nhắn lên giao diện
                            AppendMessageToUI(message);
                        }
                        else
                        {
                            Console.WriteLine("RoomID không khớp, không hiển thị tin nhắn.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi xử lý tin nhắn: {ex.Message}");
                    }
                });



                // Lắng nghe lỗi
                _clientSocket.On("error", (response) =>
                {
                    MessageBox.Show(response.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải lịch sử chat từ server.
        /// </summary>
        private async Task LoadChatHistory()
        {
            try
            {
                await _clientSocket.EmitAsync("get-messages", _roomID);

                // Lắng nghe sự kiện tải lịch sử
                _clientSocket.On("message-history", (SocketIOResponse response) =>
                {
                    try
                    {
                        string rawJson = response.ToString();
                        Console.WriteLine($"JSON thô: {rawJson}");

                        // Parse JSON lồng nhau
                        var outerList = JsonSerializer.Deserialize<List<List<ChatMessage>>>(rawJson);

                        if (outerList != null && outerList.Count > 0)
                        {
                            var historyList = outerList[0]; // Lấy mảng đầu tiên

                            if (rtbChat.IsHandleCreated)
                            {
                                rtbChat.Invoke(new Action(() =>
                                {
                                    rtbChat.Clear();
                                    foreach (var message in historyList)
                                    {
                                        AppendMessageToUI(message);
                                    }
                                }));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu tin nhắn hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Lỗi parse JSON: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cập nhật giao diện RichTextBox với tin nhắn mới.
        /// </summary>
        /// <param name="message">Đối tượng ChatMessage chứa nội dung tin nhắn.</param>
        private void AppendMessageToUI(ChatMessage message)
        {
            // Nếu Content là null hoặc empty, tránh thêm vào rtbChat
            if (!string.IsNullOrEmpty(message.Content))
            {
                if (rtbChat.IsHandleCreated)
                {
                    rtbChat.Invoke(new Action(() =>
                    {
                        Console.WriteLine($"Adding message: {message.Sender}: {message.Content}");
                        rtbChat.AppendText($"{message.Sender}: {message.Content}\n");

                        // Tự động cuộn xuống cuối khi thêm tin nhắn
                        rtbChat.ScrollToCaret();
                    }));
                }
            }
            else
            {
                if (rtbChat.IsHandleCreated)
                {
                    rtbChat.Invoke(new Action(() =>
                    {
                        Console.WriteLine($"Adding message: {message.Sender}: [No content]"); // Kiểm tra log
                        rtbChat.AppendText($"{message.Sender}: [No Content]\n");

                        // Tự động cuộn xuống cuối khi thêm tin nhắn
                        rtbChat.ScrollToCaret();
                    }));
                }
            }
        }

        /// <summary>
        /// Cập nhật danh sách thành viên trong giao diện.
        /// </summary>
        /// <param name="membersList">Danh sách thành viên.</param>
        private void UpdateMemberListUI(List<string> membersList)
        {
            if (groupboxDs.IsHandleCreated) // Kiểm tra xem handle đã được tạo chưa
            {
                groupboxDs.Invoke(new Action(() =>
                {
                    groupboxDs.Controls.Clear();
                    if (membersList != null && membersList.Count > 0)
                    {
                        int yPosition = 50;
                        foreach (var member in membersList)
                        {
                            var memberLabel = new Label
                            {
                                Text = member,
                                Location = new Point(10, yPosition),
                                AutoSize = true
                            };
                            groupboxDs.Controls.Add(memberLabel);
                            yPosition += 30;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không có thành viên nào trong nhóm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }));
            }
        }


        private async void btnDang_Click(object sender, EventArgs e)
        {
            try
            {
                string messageContent = txtChatbox.Text.Trim();
                if (string.IsNullOrEmpty(messageContent))
                {
                    MessageBox.Show("Vui lòng nhập nội dung tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newMessage = new
                {
                    RoomID = _roomID,
                    Sender = _currentUser.Username,
                    Content = messageContent,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                await _clientSocket.EmitAsync("send-message", newMessage);

                // Xóa nội dung khung nhập sau khi gửi
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
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sender")]
    public string Sender { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}

public class NewMessage
{
    [JsonPropertyName("RoomID")]
    public string RoomID { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sender")]
    public string Sender { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}



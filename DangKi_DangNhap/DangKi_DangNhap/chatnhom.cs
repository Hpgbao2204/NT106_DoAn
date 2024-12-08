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
using static Google.Apis.Requests.BatchRequest;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonException = System.Text.Json.JsonException;
using Microsoft.VisualBasic.ApplicationServices;


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

                _clientSocket.On("room-deleted", (data) =>
                {
                    try
                    {
                        // Chuyển dữ liệu thô thành JSON để xử lý
                        string raw = data.ToString();
                        Console.WriteLine($"JSON thô: {raw}");

                        // Giải mã JSON thành một mảng đối tượng
                        var parsedData = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(raw);

                        // Lấy phần tử đầu tiên của mảng
                        if (parsedData != null && parsedData.Count > 0 && parsedData[0].ContainsKey("RoomID"))
                        {
                            string roomIdDeleted = parsedData[0]["RoomID"];
                            Console.WriteLine($"RoomID bị xóa: {roomIdDeleted}");

                            // Kiểm tra nếu RoomID trùng với phòng hiện tại
                            if (roomIdDeleted == _roomID)
                            {
                                // Hiển thị thông báo và đóng form
                                MessageBox.Show("Phòng đã bị xóa bởi người quản trị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Đóng form hiện tại
                                if (this.InvokeRequired)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.Close();
                                    }));
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Dữ liệu không hợp lệ hoặc không chứa RoomID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi xử lý dữ liệu JSON: {ex.Message}");
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
        private void btnKhoTaiLieu_Click(object sender, EventArgs e)
        {
            // Tạo một instance của form khotailieu
            khotailieu ktl = new khotailieu();

            // Ẩn form hiện tại
            this.Hide();

            // Đăng ký sự kiện khi form khotailieu đóng
            ktl.FormClosed += (s, args) =>
            {
                // Hiện lại form hiện tại
                this.Show();
            };

            // Mở form khotailieu
            ktl.Show();

        }
        private async void btnRoiNhom_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn rời nhóm không?", // Nội dung thông báo
                "Xác nhận",                             // Tiêu đề
                MessageBoxButtons.YesNo,                // Các nút lựa chọn
                MessageBoxIcon.Question                 // Biểu tượng
            );

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                if (_clientSocket != null && _clientSocket.Connected)
                {
                    string messageContent = $"Người dùng {_currentUser.Username} đã rời khỏi nhóm.";

                    var newMessage = new
                    {
                        RoomID = _roomID,
                        Sender = _currentUser.Username,
                        Content = messageContent,
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    };

                    // Gửi sự kiện rời nhóm tới server
                    await _clientSocket.EmitAsync("leave-room", newMessage);

                    MessageBox.Show("Bạn đã rời nhóm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close(); // đóng Form chat nhóm
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Người dùng chọn "No"
                return;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhóm không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Gửi yêu cầu xóa nhóm đến server
                var roomId = _roomID; // Thay thế bằng ID của nhóm cần xóa
                var username = _currentUser.Username; // Thay thế bằng tên người dùng hiện tại

                _clientSocket.EmitAsync("delete-room", new
                {
                    RoomID = roomId,
                    Username = username
                });

                Console.WriteLine("Đang gửi yêu cầu xóa nhóm...");
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



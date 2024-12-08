using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Storage;
using Newtonsoft.Json;

namespace DangKi_DangNhap
{
    public partial class khotailieu : Form
    {
        private string groupID;
        public khotailieu(string roomID)
        {
            this.groupID = roomID;
            InitializeComponent();
            InitializeListView(); // Initialize ListView when the form is created
            this.Load += khotailieu_Load; // Handle form load event to fetch and display files
        }

        private async void khotailieu_Load(object sender, EventArgs e)
        {
            // Fetch files from Firebase Storage
            var files = await GetFilesFromFirebaseStorage();

            // Populate the ListView
            foreach (var file in files)
            {
                AddFileToListView(
                    file.TryGetValue("name", out var name) ? name?.ToString() : "Unknown",
                    Path.GetExtension(name?.ToString()).TrimStart('.'),
                    file.TryGetValue("updated", out var updated) ? DateTime.Parse(updated.ToString()) : DateTime.MinValue,
                    file.TryGetValue("size", out var size) ? long.Parse(size.ToString()) : 0L
                );
            }
        }

        private async Task<List<Dictionary<string, object>>> GetFilesFromFirebaseStorage()
        {
            string bucketName = "nt106-cce90.appspot.com"; // Replace with your bucket name
            var storageClient = new HttpClient();
            var fileList = new List<Dictionary<string, object>>();

            string listFilesUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o?alt=media";
            HttpResponseMessage response = await storageClient.GetAsync(listFilesUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic fileData = JsonConvert.DeserializeObject(jsonResponse);

                if (fileData?.items != null)
                {
                    foreach (var item in fileData.items)
                    {
                        if (item != null)
                        {
                            string fileName = item.name?.ToString();
                            var fileMetadata = await GetFileMetadata(bucketName, fileName);

                            var fileInfo = new Dictionary<string, object>
                            {
                                { "name", fileName },
                                { "size", fileMetadata.TryGetValue("size", out var size) ? size : 0L },
                                { "updated", fileMetadata.TryGetValue("updated", out var updated) ? updated : DateTime.MinValue }
                            };

                            fileList.Add(fileInfo);
                        }
                    }
                }
            }

            return fileList;
        }

        private async Task<Dictionary<string, object>> GetFileMetadata(string bucketName, string fileName)
        {
            var storageClient = new HttpClient();
            var fileMetadata = new Dictionary<string, object>();

            string metadataUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(fileName)}";
            HttpResponseMessage response = await storageClient.GetAsync(metadataUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic metadata = JsonConvert.DeserializeObject(jsonResponse);

                fileMetadata["size"] = metadata.size != null ? long.Parse(metadata.size.ToString()) : 0L;
                fileMetadata["updated"] = metadata.updated != null ? DateTime.Parse(metadata.updated.ToString()) : DateTime.MinValue;
            }

            return fileMetadata;
        }

        private async void btnMoFile_Click(object sender, EventArgs e)
        {
            // Tạo OpenFileDialog để chọn file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|Word documents (*.doc;*.docx)|*.doc;*.docx|Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string fileType = Path.GetExtension(filePath).TrimStart('.');
                DateTime uploadTime = DateTime.Now;
                long fileSize = new FileInfo(filePath).Length;
                txtTenFile.Text = fileName; // Giả sử bạn đã thêm TextBox tên txtTenFile
                txtPatch.Text = filePath; // Giả sử bạn đã thêm TextBox tên txtDuongDan

                try
                {
                    // Upload file lên Firebase và cập nhật progress
                    string downloadUrl = await UploadFileToFirebaseWithProgress(filePath);

                    // Thông báo thành công
                    MessageBox.Show("Upload thành công!");

                    // Hiển thị chi tiết file trong ListView
                    AddFileToListView(fileName, fileType, uploadTime, fileSize);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task<string> UploadFileToFirebaseWithProgress(string filePath)
        {
            string downloadUrl = string.Empty;
            try
            {
                string bucketName = "nt106-cce90.appspot.com"; // Thay bằng tên bucket Firebase của bạn
                string fileName = Path.GetFileName(filePath);  // Tên file
                string storageUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o?uploadType=media&name={fileName}";

                // Đọc file dưới dạng byte
                byte[] fileBytes = File.ReadAllBytes(filePath);

                using (HttpClient client = new HttpClient())
                {
                    // Thiết lập request
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, storageUrl)
                    {
                        Content = new ByteArrayContent(fileBytes)
                    };

                    // Thêm Content-Type Header
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Gửi request
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Kiểm tra kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{System.Web.HttpUtility.UrlEncode(fileName)}?alt=media";
                        MessageBox.Show("Upload thành công!\n" + responseBody);
                    }
                    else
                    {
                        string errorBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Có lỗi xảy ra: {response.StatusCode}\n{errorBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }

            return downloadUrl;
        }

        private void InitializeListView()
        {
            // Set ListView properties
            listView1.View = View.Details;
            listView1.FullRowSelect = true;

            // Add columns to ListView
            listView1.Columns.Add("File Name", 150);
            listView1.Columns.Add("File Type", 100);
            listView1.Columns.Add("Upload Date", 150);
            listView1.Columns.Add("File Size (Bytes)", 120);
        }

        private void AddFileToListView(string fileName, string fileType, DateTime uploadDate, long fileSize)
        {
            // Create a new ListViewItem
            var listViewItem = new ListViewItem(new[]
            {
                fileName,
                fileType,
                uploadDate.ToString("dd/MM/yyyy HH:mm:ss"),
                fileSize.ToString()
            });

            // Add the item to ListView
            listView1.Items.Add(listViewItem);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle ListView selection changes if needed
        }

        private void txtTenFile_TextChanged(object sender, EventArgs e)
        {

        }

        //private void txtPatch_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void btnMaHoaFile_Click(object sender, EventArgs e)
        //{

        //}

        private async void btnDang_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có file nào được chọn không
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                string fileName = selectedItem.SubItems[0].Text;

                // Hiển thị hộp thoại xác nhận
                DialogResult dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn tải xuống file '{fileName}' không?",
                                                            "Xác nhận",
                                                            MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    using (var saveDialog = new SaveFileDialog())
                    {
                        // Cấu hình SaveFileDialog
                        saveDialog.FileName = fileName;
                        saveDialog.Filter = "PDF files (*.pdf)|*.pdf|Word documents (*.doc;*.docx)|*.doc;*.docx|Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            string savePath = saveDialog.FileName;

                            try
                            {
                                string bucketName = "nt106-cce90.appspot.com"; // Tên bucket Firebase
                                string downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";

                                // Tải file từ URL và lưu vào đường dẫn đã chọn
                                using (HttpClient client = new HttpClient())
                                {
                                    var response = await client.GetAsync(downloadUrl);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        File.WriteAllBytes(savePath, fileBytes);

                                        MessageBox.Show("Tải xuống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Lỗi tải file: {response.StatusCode}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một file từ danh sách trước khi tải xuống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DangKi_DangNhap
{
    public partial class khotailieu : Form
    {
        private string groupID; // ID của nhóm

        public khotailieu()
        {
            InitializeComponent();
            InitializeListView();
            this.Load += khotailieu_Load;
        }

        private void khotailieu_Load(object sender, EventArgs e)
        {
            // Yêu cầu nhập hoặc chọn nhóm
            groupID = PromptForGroupID();
            if (string.IsNullOrWhiteSpace(groupID))
            {
                MessageBox.Show("Vui lòng nhập mã nhóm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Tải danh sách file
            LoadFilesForGroup();
        }

        private string PromptForGroupID()
        {
            using (var inputDialog = new Form())
            {
                inputDialog.Text = "Nhập mã nhóm";
                var label = new Label { Text = "Mã nhóm:", Dock = DockStyle.Top };
                var textBox = new TextBox { Dock = DockStyle.Top };
                var buttonOk = new Button { Text = "OK", Dock = DockStyle.Bottom };

                buttonOk.Click += (s, e) => inputDialog.DialogResult = DialogResult.OK;

                inputDialog.Controls.Add(buttonOk);
                inputDialog.Controls.Add(textBox);
                inputDialog.Controls.Add(label);
                inputDialog.AcceptButton = buttonOk;

                return inputDialog.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : null;
            }
        }

        private async void LoadFilesForGroup()
        {
            try
            {
                var files = await GetFilesFromFirebaseStorage();

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<Dictionary<string, object>>> GetFilesFromFirebaseStorage()
        {
            string bucketName = "nt106-cce90.appspot.com";
            string listFilesUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o?prefix=nhom-{groupID}/";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(listFilesUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to fetch file list: {response.StatusCode}");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic fileData = JsonConvert.DeserializeObject(jsonResponse);

                var fileList = new List<Dictionary<string, object>>();

                if (fileData?.items != null)
                {
                    foreach (var item in fileData.items)
                    {
                        string fileName = item.name?.ToString();
                        var fileMetadata = await GetFileMetadata(bucketName, fileName);

                        fileList.Add(new Dictionary<string, object>
                        {
                            { "name", fileName },
                            { "size", fileMetadata.TryGetValue("size", out var size) ? size : 0L },
                            { "updated", fileMetadata.TryGetValue("updated", out var updated) ? updated : DateTime.MinValue }
                        });
                    }
                }

                return fileList;
            }
        }

        private async void btnMoFile_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string fileType = Path.GetExtension(filePath).TrimStart('.');
                long fileSize = new FileInfo(filePath).Length;

                try
                {
                    string downloadUrl = await UploadFileToFirebaseWithProgress(filePath);
                    MessageBox.Show("Upload successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AddFileToListView(fileName, fileType, DateTime.Now, fileSize);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task<Dictionary<string, object>> GetFileMetadata(string bucketName, string fileName)
        {
            string metadataUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(fileName)}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(metadataUrl);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to fetch metadata for {fileName}: {response.StatusCode}");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic metadata = JsonConvert.DeserializeObject(jsonResponse);

                return new Dictionary<string, object>
         {
             { "size", metadata.size != null ? long.Parse(metadata.size.ToString()) : 0L },
             { "updated", metadata.updated != null ? DateTime.Parse(metadata.updated.ToString()) : DateTime.MinValue }
         };
            }
        }

        private async Task<string> UploadFileToFirebaseWithProgress(string filePath)
        {
            string bucketName = "nt106-cce90.appspot.com";
            string fileName = $"nhom-{groupID}/{Path.GetFileName(filePath)}";
            string storageUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o?uploadType=media&name={Uri.EscapeDataString(fileName)}";

            using (HttpClient client = new HttpClient())
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, storageUrl)
                {
                    Content = new ByteArrayContent(fileBytes)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Upload failed: {response.StatusCode}");

                return $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";
            }
        }

        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("File Name", 150);
            listView1.Columns.Add("File Type", 100);
            listView1.Columns.Add("Upload Date", 150);
            listView1.Columns.Add("File Size (Bytes)", 120);
        }

        private void AddFileToListView(string fileName, string fileType, DateTime uploadDate, long fileSize)
        {
            var listViewItem = new ListViewItem(new[]
            {
                fileName,
                fileType,
                uploadDate.ToString("dd/MM/yyyy HH:mm:ss"),
                fileSize.ToString()
            });

            listView1.Items.Add(listViewItem);
        }

        private async void btnDang_Click(object sender, EventArgs e)
        {
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    var selectedItem = listView1.SelectedItems[0];
                    string fileName = selectedItem.SubItems[0].Text;
                    string bucketName = "nt106-cce90.appspot.com";
                    string downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/nhom-{groupID}%2F{Uri.EscapeDataString(fileName)}?alt=media";
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = fileName;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string localFilePath = saveFileDialog.FileName;
                            try
                            {
                                using (HttpClient client = new HttpClient())
                                {
                                    HttpResponseMessage response = await client.GetAsync(downloadUrl);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        File.WriteAllBytes(localFilePath, fileBytes);
                                        MessageBox.Show("File đã được tải xuống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else { MessageBox.Show($"Có lỗi xảy ra: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else

                {
                    MessageBox.Show("Vui lòng chọn một file từ danh sách để tải xuống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

    }
}

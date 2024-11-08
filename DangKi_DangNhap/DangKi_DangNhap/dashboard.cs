using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace DangKi_DangNhap
{
    public partial class dashboard : Form
    {
        IFirebaseClient firebaseClient;
        private Users currentUser;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private Form currentChildForm;


        public dashboard(Users user)
        {
            InitializeComponent();
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"

            };
            firebaseClient = new FireSharp.FirebaseClient(config);
            currentUser = user;
        }

        private async void pAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                // Đặt pbAvatar không hiển thị cho đến khi ảnh được tải về
                pbAvatar.Visible = false;

                // Truy xuất ảnh base64 từ Firebase
                FirebaseResponse response = await firebaseClient.GetAsync($"Users/{currentUser.Username}/Image");
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
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dashboard_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            thongbao_form tb = new thongbao_form();
            this.Hide();
            tb.ShowDialog();
            this.ShowDialog();
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pbAvatar_Click(object sender, EventArgs e)
        {
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_zoom_Click(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái hiện tại của cửa sổ và chuyển đổi giữa phóng to và thu nhỏ lại
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized; // Phóng to cửa sổ
            }
            else
            {
                this.WindowState = FormWindowState.Normal; // Thu nhỏ lại cửa sổ về kích thước ban đầu
            }
        }

        private void btnNhomHoc_Click(object sender, EventArgs e)
        {
            NhomHoc_form tb = new NhomHoc_form();
            this.Hide();
            tb.ShowDialog();
            this.ShowDialog();
        }
    }
}
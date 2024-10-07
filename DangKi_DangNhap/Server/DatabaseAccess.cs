using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    class DatabaseAccess
    {
        private static FirebaseClient firebaseClient = Connection_Server.GetFirebaseClient();

        // Đọc danh sách tài khoản từ Firebase
        public async Task<List<TaiKhoan_Server>> DocDanhSachTaiKhoan()
        {
            var danhSachTaiKhoan = new List<TaiKhoan_Server>();

            var taiKhoanList = await firebaseClient
                .Child("TaiKhoan_Server")
                .OnceAsync<TaiKhoan_Server>();

            foreach (var item in taiKhoanList)
            {
                danhSachTaiKhoan.Add(item.Object);
            }

            return danhSachTaiKhoan;
        }

        // Kiểm tra đăng nhập từ Firebase
        public async Task<bool> KiemTraDangNhap(string taiKhoan, string matKhau)
        {
            var taiKhoanList = await firebaseClient
                .Child("TaiKhoan_Server")
                .OnceAsync<TaiKhoan_Server>();

            foreach (var item in taiKhoanList)
            {
                if (item.Object.Taikhoan == taiKhoan && item.Object.Matkhau == matKhau)
                {
                    return true;
                }
            }

            MessageBox.Show("Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        // Thêm tài khoản mới vào Firebase
        public async Task ThemTaiKhoan(string taiKhoan, string matKhau, string email)
        {
            try
            {
                await firebaseClient
                    .Child("TaiKhoan_Server")
                    .PostAsync(new TaiKhoan_Server(taiKhoan, matKhau));

                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lấy mật khẩu cho quên mật khẩu từ Firebase
        public async Task<string> LayMatKhauQuenMatKhau(string email)
        {
            var taiKhoanList = await firebaseClient
                .Child("TaiKhoan_Server")
                .OnceAsync<TaiKhoan_Server>();

            foreach (var item in taiKhoanList)
            {
                if (item.Object.Taikhoan == email) // Sử dụng email thay cho tài khoản
                {
                    return item.Object.Matkhau;
                }
            }

            MessageBox.Show("Không tìm thấy email trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        // Kiểm tra tài khoản tồn tại trên Firebase
        public async Task<bool> KiemTraTonTaiTaiKhoan(string taiKhoan)
        {
            var taiKhoanList = await firebaseClient
                .Child("TaiKhoan_Server")
                .OnceAsync<TaiKhoan_Server>();

            foreach (var item in taiKhoanList)
            {
                if (item.Object.Taikhoan == taiKhoan)
                {
                    return true;
                }
            }

            return false;
        }

        // Kiểm tra email tồn tại trên Firebase
        public async Task<bool> KiemTraTonTaiEmail(string email)
        {
            var taiKhoanList = await firebaseClient
                .Child("TaiKhoan_Server")
                .OnceAsync<TaiKhoan_Server>();

            foreach (var item in taiKhoanList)
            {
                if (item.Object.Taikhoan == email) // Sử dụng email thay cho tài khoản
                {
                    return true;
                }
            }

            return false;
        }
    }
}

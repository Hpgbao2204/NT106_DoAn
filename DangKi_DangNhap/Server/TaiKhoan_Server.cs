using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TaiKhoan_Server
    {
        private string taiKhoan;
        private string matKhau;

        public TaiKhoan_Server()
        {
            // Hàm khởi tạo mặc định không cần tham số
        }

        public TaiKhoan_Server(string taiKhoan, string matKhau)
        {
            // Hàm khởi tạo với thông tin tài khoản và mật khẩu được cung cấp
            this.taiKhoan = taiKhoan;
            this.matKhau = matKhau;
        }

        // Properties để truy cập và cập nhật tên tài khoản
        public string Taikhoan
        {
            get { return taiKhoan; }
            set { taiKhoan = value; }
        }

        // Properties để truy cập và cập nhật mật khẩu
        public string Matkhau
        {
            get { return matKhau; }
            set { matKhau = value; }
        }
    }
}

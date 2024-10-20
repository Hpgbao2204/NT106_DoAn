﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;

namespace DangKi_DangNhap
{

    public partial class login : Form
    {
        private string originalPassword = string.Empty;

        public login()
        {
            InitializeComponent();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            forgopassword forgotpass = new forgopassword();
            this.Hide();
            forgotpass.ShowDialog();
            this.Show();

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {// Lấy giá trị người dùng nhập vào từ TextBox
         // Lấy giá trị người dùng nhập vào từ TextBox
            string username = txtUsername.Text;


        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Check if the user pressed the Backspace key
            if (Control.ModifierKeys == Keys.None && txtPassword.Text.Length < originalPassword.Length)
            {
                // Remove the last character from the originalPassword if Backspace is pressed
                originalPassword = originalPassword.Substring(0, originalPassword.Length - 1);
            }
            else
            {
                // Otherwise, add the last entered character to originalPassword
                string newChar = txtPassword.Text.Length > originalPassword.Length
                                 ? txtPassword.Text.Substring(txtPassword.Text.Length - 1)
                                 : string.Empty;

                // Append the new character to the originalPassword
                originalPassword += newChar;
            }

            // Mask the TextBox input with asterisks
            txtPassword.Text = new string('*', originalPassword.Length);

            // Set the cursor to the end of the TextBox
            txtPassword.SelectionStart = txtPassword.Text.Length;
        }

        private void btn_Signup_Click(object sender, EventArgs e)
        {
            signup signup_form = new signup();
            this.Hide();
            signup_form.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Firebase configuration
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
                BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"
            };

            IFirebaseClient client = new FireSharp.FirebaseClient(config);

            if (client == null)
            {
                MessageBox.Show("Kết nối Firebase thất bại. Vui lòng kiểm tra lại cấu hình.", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = originalPassword; // Use the original password entered by the user

            // Retrieve user data from Firebase
            FirebaseResponse response = client.Get("Users/" + username);
            User user = response.ResultAs<User>(); // Deserialize the data to User object

            if (user == null)
            {
                MessageBox.Show("Không tìm thấy username.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the password matches using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (isPasswordValid)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private void control_Close_Click(object sender, EventArgs e)
        {

        }

        private void control_Minimize_Click(object sender, EventArgs e)
        {

        }
    }
}

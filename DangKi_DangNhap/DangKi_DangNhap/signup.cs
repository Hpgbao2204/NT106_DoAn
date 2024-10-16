using FireSharp.Config;
using FireSharp.Interfaces;
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
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv",
            BasePath = "https://nt106-cce90-default-rtdb.firebaseio.com/"

        };

        IFirebaseClient client;

        private void signup_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(Config);
                if (client == null)
                {
                    MessageBox.Show("Can not connect to Server. Please re-check your configuration and Internet connection.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to connect to Firebase" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private static TcpListener tcpListener;
        private static Thread listenThread;

        // Firebase client setup
        private static FirebaseClient firebaseClient;

        public Form1()
        {
            InitializeComponent();
            InitializeFirebase();
            StartServer();
        }

        private static void InitializeFirebase()
        {
            string firebaseUrl = "https://nt106-cce90-default-rtdb.firebaseio.com/";

            // Secret Key của Firebase Database (Legacy Key)
            string secret = "Thf1EHNiaoAUD1hL1NO8NlozBmCdB23d1CLAAcBv"; 

            firebaseClient = new FirebaseClient(
                firebaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(secret)
                }
            );

            MessageBox.Show("Firebase khởi tạo thành công.");
        }

        private static void StartServer()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();

            MessageBox.Show("Server is running. Waiting for connections...");
        }

        private static void ListenForClients()
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                string dataReceived = Encoding.ASCII.GetString(message, 0, bytesRead);

                // Tách dữ liệu nhận được từ client thành các tham số
                string[] requestParts = dataReceived.Split('|');

                // Xác định loại yêu cầu từ client (đăng kí hoặc đăng nhập)
                string command = requestParts[0];

                switch (command)
                {
                    case "DANGKI":
                        HandleRegistration(requestParts, clientStream).Wait();
                        break;
                    case "DANGNHAP":
                        HandleLogin(requestParts, clientStream).Wait();
                        break;
                    case "QUENMK":
                        HandleQMK(requestParts, clientStream).Wait();
                        break;
                    default:
                        SendResponse(clientStream, "INVALID_REQUEST");
                        break;
                }
            }

            tcpClient.Close();
        }

        // Xử lý yêu cầu đăng nhập
        private static async Task HandleLogin(string[] requestParts, NetworkStream clientStream)
        {
            string taiKhoan = requestParts[1];
            string matKhau = requestParts[2];

            var result = await firebaseClient
                .Child("users")
                .OnceAsync<dynamic>();

            bool found = false;
            foreach (var user in result)
            {
                if (user.Object.username == taiKhoan && user.Object.password == matKhau)
                {
                    found = true;
                    break;
                }
            }

            // Send login result to the client
            SendResponse(clientStream, found ? "LOGIN_SUCCESS" : "LOGIN_FAILED");

            if (found)
            {
                MessageBox.Show($"Tài khoản: {taiKhoan} - Đăng nhập thành công");
            }
        }

        // Xử lý yêu cầu đăng kí
        private static async Task HandleRegistration(string[] requestParts, NetworkStream clientStream)
        {
            string taiKhoan = requestParts[1];
            string matKhau = requestParts[2];
            string email = requestParts[3];

            // Kiểm tra tài khoản tồn tại trong Firebase
            var users = await firebaseClient
                .Child("users")
                .OnceAsync<dynamic>();

            foreach (var user in users)
            {
                if (user.Object.username == taiKhoan)
                {
                    SendResponse(clientStream, "TAIKHOAN_EXIST");
                    return;
                }
                if (user.Object.email == email)
                {
                    SendResponse(clientStream, "EMAIL_EXIST");
                    return;
                }
            }

            // Đăng ký tài khoản mới
            await firebaseClient
                .Child("users")
                .PostAsync(new { username = taiKhoan, password = matKhau, email = email });

            // Gửi phản hồi cho client
            SendResponse(clientStream, "DANGKI_SUCCESS");

            MessageBox.Show($"Tài khoản: {taiKhoan} - Đăng kí thành công");
        }

        // Xử lý yêu cầu quên mật khẩu
        private static async Task HandleQMK(string[] requestParts, NetworkStream clientStream)
        {
            string email = requestParts[1];

            // Tìm mật khẩu qua email
            var users = await firebaseClient
                .Child("users")
                .OnceAsync<dynamic>();

            string matKhau = null;
            foreach (var user in users)
            {
                if (user.Object.email == email)
                {
                    matKhau = user.Object.password;
                    break;
                }
            }

            if (matKhau != null)
            {
                SendResponse(clientStream, "MATKHAU|" + matKhau);
                MessageBox.Show($"Email: {email} - Lấy lại mật khẩu thành công");
            }
            else
            {
                SendResponse(clientStream, "MATKHAU_NOT_FOUND");
                MessageBox.Show($"Email: {email} - Không tìm thấy mật khẩu");
            }
        }

        private static void SendResponse(NetworkStream clientStream, string response)
        {
            byte[] responseData = Encoding.ASCII.GetBytes(response);
            clientStream.Write(responseData, 0, responseData.Length);
        }
    }
}

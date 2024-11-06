using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace Server
{
    public partial class ChatServer : Form
    {
        public ChatServer()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;

        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            server.Bind(IP);

            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(1000);
                        Socket client = server.Accept();
                        clientList.Add(client);

                        Thread recieve = new Thread(Recieve);
                        recieve.IsBackground = true;
                        recieve.Start(client);
                    }
                }
                catch (Exception ex)
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            listen.IsBackground = true;
            listen.Start();
        }

        void Recieve(Object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    //new chuyen byte sang string
                    String message = Encoding.UTF8.GetString(data);

                    foreach (Socket item in clientList)
                    {
                        if (client != null && item != client)
                        {
                            item.Send(data);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clientList.Remove(client);
                client.Close();
            }


        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}

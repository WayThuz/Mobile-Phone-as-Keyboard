using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;


namespace VirtualKeyboard {
    internal class InputReceiver {     
        public void StartListening() {
            TcpListener server = null;
            try {
                Int32 port = 8080;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port);

                server.Start();

                byte[] bytes = new byte[256];
                String data = null;

                while(true) {
                    Console.WriteLine("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;
                    while((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // 这里添加模拟键盘输入的代码
                    }

                    client.Close();
                }
            }
            catch(SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally {
                if(server != null) {
                    server.Stop();
                }
            }
        }
    }
}

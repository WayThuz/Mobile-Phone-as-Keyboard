using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKeyboard {
    public class PCServer {
        private ServerThread? serverThread;
        private bool isMessageSend;

        public void Start() {
            serverThread = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 10086);
            serverThread.Listen();
            serverThread.StartConnect();
            isMessageSend = true;
        }

        public async void Update() {
            if(serverThread == null) {
                Console.WriteLine("Server Thread is null");
                return;
            }
            serverThread.Receive();
            if(serverThread.ReceiveMessage != null) {
                Console.WriteLine("Client said: " + serverThread.ReceiveMessage);
                serverThread.ReceiveMessage = null;
            }
            if(isMessageSend) {
                isMessageSend = false;
                serverThread.Send("This is the message send from server!");
                await Task.Delay(20);
                isMessageSend = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKeyboard {
    public class ServerThread {

        private string ip;
        private int port;

        private Socket serverSocket;
        private Socket clientSocket;

        public ServerThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port) {
            serverSocket = new(family, socketType, protocolType);
            this.ip = ip;
            this.port = port;
        }

        public void Listen() {
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            serverSocket.Listen(1);
        }

        

        public void StartConnect() {
            try {
                clientSocket = serverSocket.Accept();
            }
            catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void StopConnect() {
            try {
                clientSocket.Close();
            }
            catch(Exception e) { 
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(string msg) {
            if(msg == null) throw new NullReferenceException("input message is null");
            try {
                if(clientSocket?.Connected is true) {
                    clientSocket.Send(Encoding.ASCII.GetBytes(msg));
                }
            }
            catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void Receive() {
            if(clientSocket?.Connected is true) {
                try {
                    byte[] bytes = new byte[256];
                    long dataLength = clientSocket.Receive(bytes);
                    ReceiveMessage = Encoding.ASCII.GetString(bytes);
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public string? ReceiveMessage { get; set; }
        
    }
}

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;

namespace VirtualKeyboard {
    public class PCClient {

        public delegate void ReceiveMessage(string msg);
        public ReceiveMessage receiveMsgEvents;

        private Socket clientSocket;
        private string ip;
        private int port;

        public PCClient(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port) {
            clientSocket = new(family, socketType, protocolType);
            this.ip = ip;
            this.port = port;
        }

        public void Connect(string path) {
            var process = CreateADBProcess();
            process.Start();
            process.StandardInput.WriteLine(@$"cd {path}");
            process.StandardInput.WriteLine(@"adb forward tcp:10086 tcp:10086");
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint epHost = new(ipAddress, port);
            clientSocket.Connect(epHost);
        }

        public void Update() {
            byte[] bytes = new byte[1024];
            int i;
            while((i = clientSocket.Receive(bytes)) != 0) {
                MsgReceived = Encoding.UTF8.GetString(bytes, 0, i);
                if(MsgReceived != null) {
                    receiveMsgEvents?.Invoke(MsgReceived);
                    Console.WriteLine($"message received: {MsgReceived}");
                    MsgReceived = null;
                }
            }
            Console.WriteLine("Server closed! ");
            clientSocket.Close(); //terminate 
        }

        private Process CreateADBProcess() {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            return process;
        }


        public string MsgReceived { get; set;}
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Text;

namespace VirtualKeyboard.Android {
    public class ClientThread {
        private string ip;
        private int port;

        private Socket clientSocket;

        private Thread threadConnect;
        private Thread threadReceive;

        public ClientThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port) {
            clientSocket = new(family, socketType, protocolType);
            this.ip = ip;
            this.port = port;
            ReceiveMessage = null;
        }

        public void StartConnect() {
            threadConnect = new(Accept);
            threadConnect.Start();
        }

        private void Accept() {
            try {
                Debug.Log((ip, port));
                clientSocket.Connect(IPAddress.Parse(ip), port); 
            }
            catch(Exception e) {
                Debug.LogException(e);
            }
        }

        public void StopConnect() {
            try {
                clientSocket.Close();
            }
            catch(Exception e) {
                Debug.LogException(e);
            }
        }

        public void Send(string msg) {
            if(msg == null) throw new NullReferenceException("Input message is null");
            try {
                if(clientSocket.Connected) {
                    clientSocket.Send(Encoding.ASCII.GetBytes(msg));
                }
            }
            catch(Exception e) {
                Debug.LogException(e);
            }
        }

        public void Receive() {
            bool isReceiveQuestRunning = threadReceive != null && threadReceive.IsAlive;
            if(isReceiveQuestRunning) return;
            threadReceive = new(OnReceiveMessage);
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        private void OnReceiveMessage() {
            if(clientSocket.Connected) {
                byte[] bytes = new byte[256];
                long dataLength = clientSocket.Receive(bytes);
                ReceiveMessage = Encoding.ASCII.GetString(bytes);
            }
        }

        public string ReceiveMessage { get; set; }
    }
}

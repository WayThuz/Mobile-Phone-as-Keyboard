using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace VirtualKeyboard.Android {
    public class Client : MonoBehaviour {
        private ClientThread clientThread;

        private bool isMessageSend;

        void Start() {
            Init();
        }

        private void Init() {
            clientThread = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 10086);
            clientThread.StartConnect();
            isMessageSend = true;
        }

        public async void Send(string msg) {
            clientThread.Receive();
            if(clientThread.ReceiveMessage != null) {
                Debug.Log($"Receive from server {clientThread.ReceiveMessage}");
                clientThread.ReceiveMessage = null;
            }
            if(isMessageSend) {
                isMessageSend = false;
                clientThread.Send(msg);
                await Task.Delay(20);
                isMessageSend = true;
            }
        }


        void OnApplicationQuit() {
            clientThread.StopConnect();
        }
    }
}

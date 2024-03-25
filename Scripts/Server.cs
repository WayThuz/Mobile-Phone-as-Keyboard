using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{
    private const int port = 10086;
    private const string ip = "127.0.0.1";
    private Socket serverSocket;
    private Socket clientSocket;
    private Thread threadConnect;

    void Start() {
        Init();
        threadConnect = new(StartConnect);
        threadConnect.IsBackground = true;
        threadConnect.Start();
    }

    private void Init() {
        serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
        serverSocket.Listen(1);
    }

    public void StartConnect() {
        try {
            clientSocket = serverSocket.Accept();
        }
        catch(Exception e) {
            Debug.LogException(e);
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

    void OnApplicationQuit() {
        StopConnect();
    }

    public void StopConnect() {
        try {
            clientSocket.Close();
        }
        catch(Exception e) {
            Console.WriteLine(e.ToString());
            Debug.LogException(e);
        }
    }

    public string ReceiveMessage { get; set; }
}

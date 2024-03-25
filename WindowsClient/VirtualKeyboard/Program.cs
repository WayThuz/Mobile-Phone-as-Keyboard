// See https://aka.ms/new-console-template for more information
using VirtualKeyboard;
using System.Runtime.InteropServices;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");


/* PC as server
var server = new PCServer();
server.Start();
while(true) {
    server.Update();
}
*/



InputSender sender = new InputSender();
PCClient client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 10086);
client.receiveMsgEvents += new PCClient.ReceiveMessage(sender.Send);
Console.WriteLine("Paste your ADB path");
string path = Console.ReadLine();
client.Connect(path);
client.Update();
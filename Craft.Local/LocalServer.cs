using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Craft.Net.Server;
using Craft.Net.Server.Events;
using System.Threading.Tasks;
using System.Threading;

namespace Craft.Local
{
    public class LocalServer : MinecraftServer
    {
        public bool EnableCheats { get; set; }
        public bool LanMode { get; private set; }

        public LocalServer(IPEndPoint endPoint) : base(endPoint)
        {
            ChatMessage += OnChatMessage;
            LanMode = false;
        }

        private void OnChatMessage(object sender, ChatMessageEventArgs chatMessageEventArgs)
        {
            if (chatMessageEventArgs.RawMessage.StartsWith("/"))
            {
                chatMessageEventArgs.Handled = true;
                string command = chatMessageEventArgs.RawMessage.Substring(1);
                string[] parameters = command.Split(' ');
                command = parameters[0];
                switch (command)
                {
                    case "exit":
                        Program.ExitReset.Set();
                        break;
                    case "publish":
                        OpenLocalServer();
                        break;
                }
            }
        }

        public void OpenLocalServer()
        {
            int port = ((IPEndPoint)Socket.LocalEndPoint).Port;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
            Socket.Listen(10);
            Socket.BeginAccept(AcceptConnectionAsync, null);

            PingThread = new Thread(SendLocalPings);
            PingThread.Start();

            LanMode = true;
        }

        public void StopLocalServer()
        {
            // Note: doesn't stop accepting connections, simply stops broadcasting
            if (LanMode)
                PingThread.Abort();
        }

        private Thread PingThread;
        private void SendLocalPings()
        {
            UdpClient client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            var entry = Dns.GetHostEntry(Dns.GetHostName());
            var address = entry.AddressList.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);
            SendChat("Local game hosted on " + address + ":" + ((IPEndPoint)Socket.LocalEndPoint).Port);
            while (true)
            {
                byte[] buf = Encoding.Default.GetBytes("[MOTD]" + MotD + "[/MOTD][AD]" + address + ":" +
                    ((IPEndPoint)Socket.LocalEndPoint).Port + "[/AD]");
                client.Client.SendTo(buf, new IPEndPoint(IPAddress.Parse("224.0.2.60"), 4445));
                Thread.Sleep(1500);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Craft.Net.Server;
using Craft.Net.Server.Events;

namespace Craft.Local
{
    public class LocalServer : MinecraftServer
    {
        public bool EnableCheats { get; set; }

        public LocalServer(IPEndPoint endPoint) : base(endPoint)
        {
            this.ChatMessage += OnChatMessage;
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
                }
            }
        }

        public void OpenLocalServer()
        {
            int port = ((IPEndPoint)Socket.LocalEndPoint).Port;
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
            Socket.Listen(10);
            Socket.BeginAccept(AcceptConnectionAsync, null);
        }
    }
}

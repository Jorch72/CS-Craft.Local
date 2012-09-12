using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Generation;
using Craft.Net.Server;
using System.Threading;
using Craft.Net.Server.Events;

namespace Craft.Local
{
    internal class Program
    {
        public static AutoResetEvent ExitReset;

        private static LocalServer server;

        static void Main(string[] args)
        {
            int port = 0;
            if (args.Length == 2)
                port = int.Parse(args[1]);
            string level = args[0];
            if (!Path.IsPathRooted(level))
            {
                level = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                     ".minecraft", "saves", level);
            }
            if (!level.EndsWith(".dat"))
                level = Path.Combine(level, "level.dat");

            ExitReset = new AutoResetEvent(false);

            server = new LocalServer(new IPEndPoint(IPAddress.Loopback, port));
            server.RegisterPluginChannel(new LanPluginChannel());
            server.RegisterPluginChannel(new ExitPluginChannel());
            server.AddLevel(new Level(new FlatlandGenerator(), Path.GetDirectoryName(level))); // TODO: Vanilla generator
            server.PlayerLoggedIn += ServerOnPlayerLoggedIn;
            server.OnlineMode = false;
            server.Start();
            Console.WriteLine(((IPEndPoint)server.Socket.LocalEndPoint).Port);

            ExitReset.WaitOne();
        }

        private static bool firstPlayerSet = false;
        private static void ServerOnPlayerLoggedIn(object sender, PlayerLogInEventArgs playerLogInEventArgs)
        {
            if (firstPlayerSet)
                return;
            firstPlayerSet = true;
            server.MotD = playerLogInEventArgs.Username + " - " + server.DefaultLevel.Name;
        }
    }
}

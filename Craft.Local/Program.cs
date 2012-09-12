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

namespace Craft.Local
{
    internal class Program
    {
        public static AutoResetEvent ExitReset;

        static void Main(string[] args)
        {
            int port = int.Parse(args[0]);
            string level = args[1];

            ExitReset = new AutoResetEvent(false);

            var server = new LocalServer(new IPEndPoint(IPAddress.Loopback, port));
            server.RegisterPluginChannel(new LanPluginChannel());
            server.RegisterPluginChannel(new ExitPluginChannel());
            server.AddLevel(new Level(new FlatlandGenerator(), Path.GetDirectoryName(level))); // TODO: Vanilla generator
            server.Start();

            ExitReset.WaitOne();
        }
    }
}

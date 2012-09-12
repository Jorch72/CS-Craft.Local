using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server;
using Craft.Net.Data;

namespace Craft.Local
{
    public class LanPluginChannel : PluginChannel
    {
        public override string Channel
        {
            get { return "Craft.Local.Open"; }
        }

        private LocalServer server;

        public override void ChannelRegistered(MinecraftServer server)
        {
            this.server = (LocalServer)server;
        }

        public override void MessageRecieved(MinecraftClient client, byte[] data)
        {
            server.DefaultLevel.GameMode = (GameMode)data[0];
            server.EnableCheats = data[1] == 1;
            server.OpenLocalServer();
            foreach (var c in server.Clients.Where(c => c.IsLoggedIn))
                c.Entity.GameMode = server.DefaultLevel.GameMode;
        }
    }
}

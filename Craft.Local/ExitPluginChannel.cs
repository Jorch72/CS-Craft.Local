using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server;

namespace Craft.Local
{
    public class ExitPluginChannel : PluginChannel
    {
        public override string Channel
        {
            get { return "Craft.Local.Exit"; }
        }

        public override void ChannelRegistered(MinecraftServer server)
        {
        }

        public override void MessageRecieved(MinecraftClient client, byte[] data)
        {
            Program.ExitReset.Set();
        }
    }
}

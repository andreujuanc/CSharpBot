using System;
using System.Collections.Generic;
using System.Text;

namespace IrcBot
{
    public class PluginList : AddOnBase
    {
        public PluginList()
        {
            IsService = true;
        }

        public override void ProcessMenssage(Nexus.IRC.IrcMessage message, string[] args)
        {
            try
            {
                switch (args[1])
                {
                    case "list":
                        this.IrcBot.SendChannelMessage("*** Listing Addons ***");
                        int i = 0;
                        foreach (string name in this.IrcBot.AddonsList.Keys)
                        {
                            this.IrcBot.SendChannelMessage("*** " + (i + 1) + ".- " + name + ((this.IrcBot.AddonsList[name].IsStarted) ? "  ( Running )" : ""));
                            i++;
                        }
                        break;
                    case "start":
                        if (this.IrcBot.AddonsList.ContainsKey(args[2]))
                            this.IrcBot.AddonsList[args[2]].Start();
                        this.IrcBot.SendChannelMessage(" >> " + args[2] + " started.");
                        break;
                    case "stop":
                        if (this.IrcBot.AddonsList.ContainsKey(args[2]))
                            this.IrcBot.AddonsList[args[2]].Stop();
                        this.IrcBot.SendChannelMessage(" >> " + args[2] + " stoped.");
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        public override string GetHelp()
        {
            return "Usage: .plugin <action> [parameters] ";
        }
    }
}

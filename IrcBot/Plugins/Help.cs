using System;
using System.Collections.Generic;
using System.Text;

namespace IrcBot.Plugins
{
    public class Help : AddOnBase
    {
        public Help()
        {
            IsService= true;
        }

        public override void ProcessMenssage(Nexus.IRC.IrcMessage message, string[] args)
        {
            string[] parameters = message.Message.Split(' ');
            if (parameters.GetLength(0) > 1)
            {
                try
                {
                    this.IrcBot.SendChannelMessage(this.IrcBot.AddonsList[parameters[1]].GetHelp());
                }
                catch 
                {
                    this.IrcBot.SendChannelMessage(GetGenericHelp());
                }
            }
            else
            {
                this.IrcBot.SendChannelMessage(GetGenericHelp());
            }
        }

        public string GetGenericHelp() 
        {
            string result = "For more information about a specific command, type .help <command>\n";
            result += "Command List:";
            foreach (string command in this.IrcBot.AddonsList.Keys)
            {
                result += " " + command;
            }
            return result;
        }

        public override string GetHelp()
        {
            return "Shows informacion and usage about a specified command.\nUsage: .help <command>\nEx:.help wiki";
        }
    }
}

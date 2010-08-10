using System;
using System.Collections.Generic;
using System.Text;

namespace IrcBot.Plugins
{
    public class IrcManagement : AddOnBase
    {
        public IrcManagement()
        {
            IsService = true;
            SubscribeOnDataReceived = true;
        }

        public override string GetHelp()
        {
            return "";
        }
    }
}

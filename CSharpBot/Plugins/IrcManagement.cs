using System;
using System.Collections.Generic;
using System.Text;
namespace CSharpBot.Plugins
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

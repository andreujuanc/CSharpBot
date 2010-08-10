using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Nexus.IRC;


namespace IrcBot
{
    public interface IAddOn
    {
        bool IsStarted{ get; }
        void Stop();
        void Start();
        void Init(IrcBot ircBot);
        void Client_ProcessedDataReceived(object sender, IrcDataEventArgs e);
        bool IsService { get; set; }
        bool SubscribeOnDataReceived { get; }
        void ProcessMenssage(IrcMessage message, string[] args);
        string GetHelp();
    }
}

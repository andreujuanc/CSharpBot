using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IrcBot
{
    public class AntiFloodPlugin : AddOnBase
    {
        long m_bytesSent = 0;
        Timer m_timer;

        public AntiFloodPlugin() 
        {
            IsService =false;    
            SubscribeOnDataReceived = true;
        }


        void TimerEvent(object obj) 
        {
            foreach (string username in this.IrcBot.MessagesByUser.Keys)
            {
                int count = this.IrcBot.MessagesByUser[username].Count;
                if ( count > 3 && count < 6 ) 
                {
                    SendMessage(username + ", No envíe tantos mensajes o será botado o baneado del canal.");
                }
                if (count > 5) 
                {
                    SendMessage("El usuario "+ username + " ha sido botado del canal.");
                    this.IrcBot.KickUser(username, "Flooding");
                }
                this.IrcBot.MessagesByUser[username].Clear();
            }

            if (m_bytesSent > 50)
            {
                this.IrcBot.Invoke(new ThreadStart(
                delegate 
                {
                    SendMessage("En espera...");
                    Thread.Sleep(5000);
                }));
                m_bytesSent = 0;
            }
        }

        public override void OnStart()
        {
             this.IrcBot.SendingMessage += new Nexus.IRC.IrcDataEventHandler(ircBot_SendingMesssage);
             m_timer = new Timer(new TimerCallback(TimerEvent), null, 1, 5000);
        }
        public override void OnStop()
        {
            this.IrcBot.SendingMessage -= new Nexus.IRC.IrcDataEventHandler(ircBot_SendingMesssage);
        }

        private void ircBot_SendingMesssage(object sender, Nexus.IRC.IrcDataEventArgs e)
        {
            foreach (string user in this.IrcBot.MessagesByUser.Keys)
            {
                if (this.IrcBot.MessagesByUser[user].Count >10) 
                {
                    e.Cancel = true;
                }
            }
        }

        private void SendMessage(string message)
        {
            this.IrcBot.SendChannelMessage(message);
        }
        public override string GetHelp()
        {
            return "";
        }
    }
}

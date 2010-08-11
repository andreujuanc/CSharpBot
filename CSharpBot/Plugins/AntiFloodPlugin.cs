/*Copyright (c) 2010, Juan C. Andreu
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.
3. All advertising materials mentioning features or use of this software
   must display the following acknowledgement:
   This product includes software developed by Juan C. Andreu.
4. Neither the name of the Juan C. Andreu nor the
   names of its contributors may be used to endorse or promote products
   derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY Juan C. Andreu ''AS IS'' AND ANY
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL Juan C. Andreu BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

CONTRIBUTORS:
   RAUL338
*/

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IrcClient;

namespace CSharpBot.Plugins
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
             this.IrcBot.SendingMessage += new IrcDataEventHandler(ircBot_SendingMesssage);
             m_timer = new Timer(new TimerCallback(TimerEvent), null, 1, 5000);
        }
        public override void OnStop()
        {
            this.IrcBot.SendingMessage -= new IrcDataEventHandler(ircBot_SendingMesssage);
        }

        private void ircBot_SendingMesssage(object sender, IrcClient.IrcDataEventArgs e)
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

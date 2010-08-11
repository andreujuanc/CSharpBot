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
﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace IrcClient
{
    
    public class Client
    {
        private TcpClient m_tcpCliet= new TcpClient();
        public event  IrcDataEventHandler DataReceived;
        StreamReader m_reader = null;
        StreamWriter m_writer = null;
        Thread m_thread = null;
        string m_channel;
        string m_server;
        int m_port;

        public Client(string server, int port) 
        {
            m_server = server;
            m_port = port;
            
        }
        protected void OnDataReceived(string data) 
        {
            if (DataReceived != null)
                DataReceived(this, new IrcDataEventArgs(new IrcMessage(data)));
        }

        private void ReadData() 
        {
            while (true)
            {
                string data = m_reader.ReadLine();
                if(data!=null)
                    OnDataReceived(data);
            }
        }

        public void Connect()
        {
            m_tcpCliet.Connect(m_server,m_port);
            m_writer = new StreamWriter(m_tcpCliet.GetStream());
            m_reader = new StreamReader(m_tcpCliet.GetStream());
            m_thread = new Thread(new ThreadStart(ReadData));
            m_thread.Start();
        }

        public bool Connected 
        {
            get { return m_tcpCliet.Connected; }
        }
        public void SendData(string data) 
        {
            m_writer.WriteLine(data);
            m_writer.Flush();
        }
        public void JoinChannel(string channel)
        {
            m_channel = channel;
            SendData("JOIN " + channel);
        }

        public void JoinUserName(string username)
        {
            SendData("USER " + username);
        }

        public void SetNick(string nick)
        {
            SendData("NICK " + nick);
        }

        public void Quit()
        {
            SendData("QUIT :Chau!!");
            m_thread.Interrupt();
            m_thread.Abort();
        }

        public void SendMessage(IrcMessage message)
        {
            SendData(message.ToString());
        }

        public void KickUser(string username,string reason)
        {
            //KICK #Finnish John :Speaking English
            SendData("KICK "+ this.m_channel+ " " + username + " :" + reason);
        }
    }
}

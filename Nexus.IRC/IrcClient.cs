using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace Nexus.IRC
{
    
    public class IrcClient
    {
        private TcpClient m_tcpCliet= new TcpClient();
        public event  IrcDataEventHandler DataReceived;
        StreamReader m_reader = null;
        StreamWriter m_writer = null;
        Thread m_thread = null;
        string m_channel;
        string m_server;
        int m_port;

        public IrcClient(string server, int port) 
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

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
using IrcClient;
using System.Threading;
using System.Diagnostics;
using CSharpBot.Plugins;
namespace CSharpBot
{
   
    public class IrcBot
    {
        public event IrcDataEventHandler DataReceived;
        public event IrcDataEventHandler ProcessedDataReceived;
     
        public event IrcDataEventHandler SendingMessage;
        public event IrcDataEventHandler MesssageSent;
        private Dictionary<string, Queue<IrcMessage>> m_messagesByUser = new Dictionary<string, Queue<IrcMessage>>();

        public Dictionary<string, Queue<IrcMessage>> MessagesByUser 
        {
            get 
            {
                return m_messagesByUser;
            }
        }

        Dictionary<string, string> m_tagsKey = new Dictionary<string, string>();
        Dictionary<string, IAddOn> m_addOns = new Dictionary<string, IAddOn>();

        public Dictionary<string, IAddOn> AddonsList 
        {
            get { return m_addOns; }
        }

        public string Nick 
        {
            get { return this.m_nick; }
            private set { m_nick = value; }
        }

        public Random Random 
        {
            get { return m_rand; }
        }

        Random m_rand = null;
        Client m_client;
        string m_channel;
        string m_nick;

        public IrcBot(string server, int port)
        {
            LoadAllAddOns();

            m_client = new Client(server, port);
            m_client.DataReceived += new IrcDataEventHandler(m_client_DataReceived);
            m_rand = new Random(DateTime.Now.Millisecond);
        }

        private void LoadAllAddOns()
        {
            m_addOns.Add("help", new Help());
            m_addOns.Add("plugin", new PluginList());
            m_addOns.Add("hangman", new HangmanGame());
            m_addOns.Add("antiflood", new AntiFloodPlugin());
            m_addOns.Add("talking", new Talking());
            m_addOns.Add("wiki", new WikipediaPlugin());
            ConfigureAddons();
        }

        private void ConfigureAddons()
        {
            foreach (string addonName in m_addOns.Keys)
            {
                IAddOn addOn = m_addOns[addonName];
                if (addOn.IsStarted)
                    continue;
                addOn.Init(this);

                if (addOn.IsService)
                     StartAddOn(addOn);

                if (addOn.SubscribeOnDataReceived)
                    ProcessedDataReceived += new IrcDataEventHandler(addOn.Client_ProcessedDataReceived);
            }
        }

        void m_client_DataReceived(object sender, IrcDataEventArgs e)
        {
            OnDataReceived(e.Data);
        }

        public void Start() 
        {
            m_client.Connect();
        }

        public bool Connected 
        {
            get { return m_client.Connected; }
        }

        protected void OnDataReceived(string data)
        {
            if (DataReceived != null)
                DataReceived(this, new IrcDataEventArgs(new IrcMessage(data)));
            ProcessData(data);
            data = String.Empty;
        }

        private void OnProcessedDataReceived(object sender, IrcDataEventArgs e) 
        {
            if (ProcessedDataReceived != null)
                ProcessedDataReceived(sender, e);
        }

        private void ProcessData(string data)
        {
            if (data == null)
                return;
           //Now all the data is processed in IrcMessage Class.

            ProcessMessage(new IrcMessage(data));
            
        }

        private void ProcessMessage(IrcMessage message)
        {
            if (message.From == null || message.Type == MessageType.Other || message.From == "")
                return;
            if (!m_messagesByUser.ContainsKey(message.From))
                m_messagesByUser.Add(message.From, new Queue<IrcMessage>(10));

            m_messagesByUser[message.From].Enqueue(message);
            OnProcessedDataReceived(this, new IrcDataEventArgs(message));

            if (message.Message.ToLower().StartsWith("."))
            {
                foreach (string key in this.AddonsList.Keys)
                {
                    if (message.Message.StartsWith("." + key))
                    {
                        string[] args = message.Message.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        this.AddonsList[key].ProcessMenssage(message,args);
                        break;
                    }
                }
            }

            else
            {
                switch (message.Type)
                {
                    case MessageType.JOIN:
                        SendChannelMessage("Everybody say Hi to: " + message.From);
                        break;
                    case MessageType.PRIVMSG:
                        break;
                    case MessageType.PING:
                        throw new Exception("Poner el codigo aca");
                        break;
                    default:
                        break;
                }
            }
        }

        private void CheckForTag(string message)
        {
            if (message.ToLower().Contains("start") || message.ToLower().Contains("iniciar") || message.ToLower().Contains("inicia"))
            {
                foreach (string key in m_tagsKey.Keys)
                {
                    if (message.ToLower().Contains(key.ToLower()))
                    {
                        StartAddOn(m_addOns[m_tagsKey[key]]);
                    }
                }
            }
            if (message.ToLower().Contains("stop") || message.ToLower().Contains("detener") || message.ToLower().Contains("deten"))
            {
                foreach (string key in m_tagsKey.Keys)
                {
                    if (message.ToLower().Contains(key.ToLower()))
                    {
                        StopAddOn(m_addOns[m_tagsKey[key]]);
                    }
                }
            }
        }

        private void StopAddOn(IAddOn iAddOn)
        {
            iAddOn.Stop();
        }

        private void StartAddOn(IAddOn iAddOn)
        {
            iAddOn.Start();
        }

        private string GetRandomMessage(List<string> list, params string[] args)
        {
            return string.Format(list[m_rand.Next(0, list.Count)], args);
        }

        internal bool Contains(string message, IEnumerable<string> wordlist, out string found)
        {
            foreach (string word in wordlist)
            {
                if (message.ToLower().Contains(word.ToLower()))
                {
                    found = word;
                    return true;
                }
            }
            found = String.Empty;
            return false;
        }
        internal void SendChannelMessage(string message)
        {
            SendChannelMessage(m_channel, message);
        }
        internal void SendChannelMessage(string channel, string message)
        {
            string[] messages = message.Split('\n');
            foreach (string msg in messages)
            {
                SendMessage(new IrcMessage(msg, channel, this.Nick));
            }
            
        }

        private void SendMessage(IrcMessage message)
        {
            IrcDataEventArgs e = new IrcDataEventArgs(message);
            if (SendingMessage != null)
                SendingMessage(this, e);
            if (!e.Cancel)
            {
                m_client.SendMessage(message);
                if (MesssageSent != null)
                    MesssageSent(this, new IrcDataEventArgs(message));
            }
        }


        public void JoinChannel(string channelName)
        {
            m_channel = channelName;
            m_client.JoinChannel(channelName);
        }

        public void SetUserName(string username)
        {
            m_client.JoinUserName(username);
        }

        public void SetNick(string nick)
        {
            Nick = nick;
            m_client.SetNick(nick);
        }

        internal void Quit()
        {
            m_client.Quit();
        }

        internal void Invoke(Delegate p)
        {
            p.DynamicInvoke(null);
        }

        internal void KickUser(string username, string reason)
        {
            m_client.KickUser(username, reason);
        }

        internal void SendData(string data)
        {
            try
            {
                if (data.StartsWith("/"))
                {
                    m_client.SendData(data.Substring(1));
                    return;
                }
            }
            catch { }
            this.SendChannelMessage(data);
        }
    }
}

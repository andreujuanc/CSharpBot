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

using System;
using System.Collections.Generic;
using System.Text;

namespace IrcClient
{
    public class IrcMessage
    {
        private string m_message;
        private string m_to;
        private string m_from;
        private string m_data;
        private MessageType m_type;
        private bool m_handled = false;


        public string Message { get { return m_message; } set { m_message = value; } }
        public string To { get { return m_to; } set { m_to = value; } }
        public string From { get { return m_from; } set { m_from = value; } }
        public string RawData { get { return m_data; } set { m_data = value; } }
        public MessageType Type { get { return m_type; } set { m_type = value; } }
        public bool Handled
        {
            get { return m_handled; }
            set { m_handled = true; }
        }


        public IrcMessage(string message, string to, string from) 
        {
            m_message = message;
            m_to = to;
            m_from = from;
        }
        public IrcMessage(string data)
        {
            m_data = data;
            
            int first = data.IndexOf(' ');

            string header = data.Substring(0, first);
            

            string content = data.Substring(first + 1);

            if (data.StartsWith(":") && header.Contains("!"))
                m_from = data.Substring(1, data.IndexOf("!") - 1);
            
            string typeName = String.Empty;
            if (header != "PING")
                typeName = content.Substring(0, content.IndexOf(' '));
            else
                typeName = "PING";

            string temp = content.Substring(content.IndexOf(' ')+1);

            
            
            
            

            switch (typeName)
            {
                case "JOIN":
                    m_type = MessageType.JOIN;
                    m_to = temp.Substring(1);
                    m_message = String.Empty;
                    break;
                case "PRIVMSG":
                    m_type = MessageType.PRIVMSG;
                    m_to = temp.Substring(0, temp.IndexOf(' '));
                    m_message = temp.Substring(m_to.Length + 2);
                    break;
                case "NOTICE":
                    m_type = MessageType.NOTICE;
                    m_to = temp.Substring(0, temp.IndexOf(':'));
                    m_message = temp.Substring(m_to.Length + 2);
                    break;
                case "PING":
                    m_type = MessageType.PING;
                    m_to = temp.Substring(0, temp.IndexOf(':'));
                    m_message = temp.Substring(m_to.Length + 2);
                    break;
                default:
                    m_to = String.Empty;
                    m_from = String.Empty;
                    m_message = String.Empty;
                    break;
            }

            

            //if (content.StartsWith("JOIN :")) 
            //{
            //    SendChannelMessage("Everybody say Hi to: " + m_from);
            //}
            //if (content.StartsWith("PRIVMSG"))
            //{
            //    m_message = m_from = data.Substring(data.IndexOf("PRIVMSG")+1);
            //}            //if (data.EndsWith()
            //{
            //    string nickname = data.Substring(1, data.IndexOf("!") - 1);

            //}
            //if () 
            //{
            //    string[] data2 = data.ToUpper().Split(new string[] { "PRIVMSG " + m_channel.ToUpper() + " :" }, StringSplitOptions.RemoveEmptyEntries);
            //    if (data2.Length > 1)
            //    {
            //        string nickname = data.Substring(1, data.IndexOf("!") - 1);
            //        string message = data2[1];
        }

        public override string ToString()
        {
            return "PRIVMSG " + m_to + " :" + m_message;
        }
    }
}

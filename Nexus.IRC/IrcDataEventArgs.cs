using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.IRC
{
    public delegate void IrcDataEventHandler(object sender, IrcDataEventArgs e);
    public class IrcDataEventArgs : EventArgs
    {
        private string m_data;
        private bool m_handled = false;
        public bool Handled
        {
            get { return m_handled; }
            set { m_handled = true; }
        }
        IrcMessage m_message;
        private bool m_cancel;

        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }
        public string Data
        {
            get { return m_message.RawData; }
            set { m_message.RawData = value; }
        }

        public string Message
        {
            get { return m_message.Message; }
            set { m_message.Message = value; }
        }
        public string From
        {
            get { return m_message.From; }
            set { m_message.From = value; }
        }

        public string To
        {
            get { return m_message.To; }
            set { m_message.To = value; }
        }

        public IrcDataEventArgs(IrcMessage message)
        {
            m_message = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using IrcClient;
namespace CSharpBot
{
    public abstract class AddOnBase : IAddOn
    {
        #region IAddOn Members
        public IrcBot IrcBot { get; set; }
        private bool m_started = false;
        private bool m_isService = false;
        private bool m_subscribeOnDataReceived = false;

        public bool IsStarted
        {
            get
            {
                return m_started;
            }
        }

        public void Init(IrcBot ircBot) 
        {
            IrcBot = ircBot;
        }

        public void Start()
        {
            
            m_started = true;
            OnStart();
        }

        public void Stop()
        {
            m_started = false;
            OnStop();
        }

        public virtual void OnStart() { }

        public virtual void OnStop() { }

      
        public bool IsService
        {
            get { return m_isService; }
            set { m_isService = value; }
        }

        public bool SubscribeOnDataReceived
        {
            get { return m_subscribeOnDataReceived; }
            internal set { m_subscribeOnDataReceived = value; }
        }

        /// <summary>
        /// This method is called when a message is readed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public virtual void ProcessMenssage(IrcMessage message, string[] args) 
        {
            
        }

        /// <summary>
        /// This method is called just when the plugin is called, Ex: .help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Client_ProcessedDataReceived(object sender,IrcClient.IrcDataEventArgs e)
        {
            
        }

        public abstract string GetHelp() ;

        #endregion
    }
}

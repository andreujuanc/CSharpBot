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
*/﻿
	
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

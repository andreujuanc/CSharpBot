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

namespace IrcClient
{
    public delegate void IrcDataEventHandler(object sender, IrcDataEventArgs e);
    public class IrcDataEventArgs : EventArgs
    {
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

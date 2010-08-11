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
namespace CSharpBot.Plugins
{
    public class Help : AddOnBase
    {
        public Help()
        {
            IsService= true;
        }

        public override void ProcessMenssage(IrcClient.IrcMessage message, string[] args)
        {
            string[] parameters = message.Message.Split(' ');
            if (parameters.GetLength(0) > 1)
            {
                try
                {
                    this.IrcBot.SendChannelMessage(this.IrcBot.AddonsList[parameters[1]].GetHelp());
                }
                catch 
                {
                    this.IrcBot.SendChannelMessage(GetGenericHelp());
                }
            }
            else
            {
                this.IrcBot.SendChannelMessage(GetGenericHelp());
            }
        }

        public string GetGenericHelp() 
        {
            string result = "For more information about a specific command, type .help <command>\n";
            result += "Command List:";
            foreach (string command in this.IrcBot.AddonsList.Keys)
            {
                result += " " + command;
            }
            return result;
        }

        public override string GetHelp()
        {
            return "Shows informacion and usage about a specified command.\nUsage: .help <command>\nEx:.help wiki";
        }
    }
}

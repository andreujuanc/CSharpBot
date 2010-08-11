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
using System.Threading;
using IrcClient;
namespace CSharpBot
{
    class Program
    {
        static IrcBot bot;
        static bool m_run = true;
        static void Main(string[] args)
        {
            //irc.freenode.net : 6667
            
			// D4N93R & R4U1 R0CK5
			
            string server = "irc.freenode.net";
            bot = new IrcBot(server, 6667);
            bot.DataReceived += new IrcDataEventHandler(bot_DataReceived);
            Console.WriteLine("Connecting to: " + server);
            bot.Start();
            if (bot.Connected) 
            {
                Console.WriteLine("Connected...");


                bot.SetUserName("CSharpBot 8 * Juan");

                bot.SetNick("CSharpBot");

                bot.JoinChannel("#elhacker.net");
               // bot.JoinChannel("#CSharpBotTest");
                //bot.JoinChannel("#botPerlTest");
            }
            while (m_run)
            {
                bot.SendData(Console.ReadLine());
            }
            
        }

        static void bot_DataReceived(object sender, IrcClient.IrcDataEventArgs e)
        {
            Console.WriteLine(e.Data);

            if (e.Data != null && e.Data.Contains("chao bot, no te quiero"))
            {
                bot.Quit();
                m_run = false;
                
            }
 
        }
    }
}
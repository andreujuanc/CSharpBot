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
using System.Diagnostics;
using IrcClient;
namespace CSharpBot.Plugins
{
    public class HangmanGame : AddOnBase
    {
        List<string> m_hangmanWords = new List<string>();
        string m_selectedWord = String.Empty;
        string m_completedWord = String.Empty;
        Random m_rand = null;


        public HangmanGame() 
        {
            m_hangmanWords.AddRange(new string[] { "POINTER", "INTEGER", "MACRO", "TEMPLATE", "DELEGATE", "FUNCTION" });
            m_rand = new Random(DateTime.Now.Millisecond);
            IsService = false;
            SubscribeOnDataReceived = false;
        }

        public override void OnStart()
        {
            NewGame();
        }

        public override void Client_ProcessedDataReceived(object sender, IrcDataEventArgs e) 
        {
            string message = e.Data;
            if (message.ToLower().Contains("letra"))
            {
                string letra = message.Substring(6, 1).ToUpper();
                if (m_selectedWord.ToUpper().Contains(letra.ToUpper()))
                {
                    for (int i = 0; i < m_selectedWord.Length; i++)
                    {
                        if (m_selectedWord[i] == letra[0]) 
                        {
                            m_completedWord = m_completedWord.Remove(i*2, 1);
                            m_completedWord = m_completedWord.Insert(i*2, letra);
                        }
                    }
                    SendMessage("*** Si contiene la letra: " + letra);
                    if (!m_completedWord.Contains("_"))
                    {
                        SendMessage("**********************");
                        SendMessage("Thank you for playing!");
                        SendMessage("**********************");
                    }
                }
                else
                    SendMessage("*** Lo siento, No contiene la letra: " + letra);
                SendMessage("***" + m_completedWord + "***");
            }
        }

       
        public void NewGame()
        {
            m_selectedWord = m_hangmanWords[m_rand.Next(0, m_hangmanWords.Count)];
            for (int i = 0; i < m_selectedWord.Length; i++)
            {
                m_completedWord += "_ ";
            }
            SendMessage("**** STARTING HANGMAN ****");
            SendMessage("**** Guess this word: " + m_completedWord);
        }
        public string[] GetTags() 
        {
            return new string[] { "ahorcado", "hangman" };
        }
        private void SendMessage(string message) 
        {
            this.IrcBot.SendChannelMessage(message);
        }

        public override string GetHelp()
        {
            return "";
        }
    }
}

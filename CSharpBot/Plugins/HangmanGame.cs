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

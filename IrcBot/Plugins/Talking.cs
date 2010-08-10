using System;
using System.Collections.Generic;
using System.Text;

namespace IrcBot
{
    public class Talking : AddOnBase
    {

        Dictionary<string, List<string>> m_conversation = new Dictionary<string, List<string>>();
        int m_mindStatus = 0;

        public Talking() 
        {
            m_conversation.Add("hi", new List<string>(new string[] { "Hi, {FROM}", "Hello, {FROM}", "Hey, {FROM}" }));
            m_conversation.Add("hello", new List<string>(new string[] { "Hi, {FROM}", "Hello, {FROM}", "Hey, {FROM}" }));
            m_conversation.Add("how are you", new List<string>(new string[] { "I'm {STATE} {FROM}, and you?" }));
            m_conversation.Add("how r u", new List<string>(new string[] { "I'm {STATE} {FROM}, and you?" }));
            SubscribeOnDataReceived = true;
            IsService = false;
        }

        public string GetMindStatus() 
        {

            if (m_mindStatus == 0)
                return "Ok";
            if (m_mindStatus > 0 && m_mindStatus < 10)
                return "Good";
            if (m_mindStatus > 9)
                return "Great! ^^";
            if (m_mindStatus < 0)
                return "not good :(";

            return "myself";
        }

        string m_lastQuestion = "";
        string answerTO = "";
        public override void Client_ProcessedDataReceived(object sender, Nexus.IRC.IrcDataEventArgs e)
        {
            if (!IsStarted)
                return;
            string foundWord = String.Empty;
            if (this.IrcBot.Contains(e.Message, m_conversation.Keys, out foundWord))
            {
                string response = m_conversation[foundWord.ToLower()][this.IrcBot.Random.Next(0, m_conversation[foundWord.ToLower()].Count)];
                if (response.Contains("{STATE}"))
                    response = response.Replace("{STATE}", GetMindStatus());
                if (response.Contains("{FROM}"))
                    response = response.Replace("{FROM}", e.From);
                this.IrcBot.SendChannelMessage(response);
                m_mindStatus++;
            }
            else if (this.IrcBot.Contains(e.Message.ToLower(), new string[] { "answer" }, out foundWord))
            {
                if (m_lastQuestion == "" || answerTO != e.From)
                    return;
                int r = e.Message.IndexOf("answer");

                if (!m_conversation.ContainsKey(m_lastQuestion.ToLower()))
                    m_conversation.Add(m_lastQuestion.ToLower(), new List<string>());

                string answer = e.Message.Substring(r + 7);
                if (answer.Contains(e.From))
                    answer = answer.Replace(e.From, "{FROM}");
                m_conversation[m_lastQuestion.ToLower()].Add(answer);

                string response = "Thank you, {FROM}";
                if (response.Contains("{FROM}"))
                    response = response.Replace("{FROM}", e.From);
                answerTO = "";
                this.IrcBot.SendChannelMessage(response);
                
            }
            else
            {
                if (e.Handled || answerTO != "")
                    return;
                if (e.Message.ToLower().Contains("bot"))
                    m_lastQuestion = e.Message.Replace("bot", "");
                if (e.Message.ToLower().Contains(this.IrcBot.Nick))
                    m_lastQuestion = e.Message.Replace(this.IrcBot.Nick, "");

                if(m_lastQuestion.EndsWith(" "))
                    m_lastQuestion = m_lastQuestion.Substring(0, m_lastQuestion.Length-1);

                if (m_lastQuestion.EndsWith(", "))
                    m_lastQuestion = m_lastQuestion.Substring(0, m_lastQuestion.Length - 2);

                if (m_lastQuestion.StartsWith(" "))
                    m_lastQuestion = m_lastQuestion.Substring(1, m_lastQuestion.Length-1);

                if (m_lastQuestion.StartsWith(", "))
                    m_lastQuestion = m_lastQuestion.Substring(2, m_lastQuestion.Length - 1);

                string response = "{FROM}, I don't know what to say, Can you tell me the anwser please?";
                answerTO = e.From;
                if (response.Contains("{FROM}"))
                    response = response.Replace("{FROM}", e.From);
                this.IrcBot.SendChannelMessage(response);
            }

        }
        public override string GetHelp()
        {
            return "";
        }
       
    }
}

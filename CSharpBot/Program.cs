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

                //bot.JoinChannel("#elhacker.net");
               // bot.JoinChannel("#CSharpBotTest");
                bot.JoinChannel("#botPerlTest");
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Nexus.IRC;
using System.Data;
using System.Text.RegularExpressions;

namespace IrcBot
{
    public class WikipediaPlugin : AddOnBase
    {
        public WikipediaPlugin() 
        {
            SubscribeOnDataReceived = false;
            IsService = false;
        }

        string searchUrl = "http://en.wikipedia.org/w/api.php?action=opensearch&format=xml&search=";
        string linksUrl = "http://en.wikipedia.org/w/api.php?action=query&prop=links&format=xml&titles=";
        string pageUrl = "http://en.wikipedia.org/w/api.php?action=query&prop=revisions&rvprop=content&format=xml&titles=";

        private void GetWikiData(string args)
        {
            int fTmp = args.IndexOf(' ');
            int sTmp = args.IndexOf(' ', fTmp + 1);

            string action = args.Substring(0, fTmp);
            string query = args.Substring(fTmp);
            
            try
            {
                XmlDocument result;
                switch (action)
                {
                    case "search":
                        result = GetResultStream(searchUrl + query);
                        ProcessSearch(result);
                        return;
                    case "links":
                        result = GetResultStream(linksUrl + query);
                        return;
                    case "view":
                        ProcessPage(query);
                        return;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.IrcBot.SendChannelMessage("Error while retrieve from Wikipedia. " + ex.ToString());
            }
        }

        private void ProcessPage(string query)
        {
            
        }

        private void ProcessSearch(XmlDocument result)
        {
            XmlNodeList nodes = result.DocumentElement["Section"].ChildNodes;
            if (nodes.Count > 0)
            {
                this.IrcBot.SendChannelMessage("Results:");
                foreach (XmlNode node in nodes)
                {
                    this.IrcBot.SendChannelMessage(" *" + node["Text"].InnerText);
                }
            }
        }

        private XmlDocument GetResultStream(string query) 
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(query);
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.UserAgent = ".NET Framework Example Client";
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();

            XmlReader xmlreader = new XmlTextReader(responseStream);
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlreader);

            xmlreader.Close();
            webResponse.Close();
            return doc;
        }

        private int GetArgValue(string[] args, string argumentPrefix)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
    			{
                    if (args[i] == argumentPrefix) 
                    {
                        return Convert.ToInt32(args[i + 1]);
                    }
                }
            }
            catch 
            {
                this.IrcBot.SendChannelMessage(GetHelp());
            }
            return -1;
        }

        public  override void ProcessMenssage(IrcMessage message, string[] args)
        {
            if (!IsStarted)
                return;
            if (message.Message.StartsWith(".wiki"))
            {
                string query = message.Message.Substring(6);
                GetWikiData(query);
            }
        }

        public override string GetHelp()
        {
            return "This plugin get info from wikipedia's site.\nUsage: .wiki <command> <query>[[-p <PI> -l <LI>] | [-s <[1|2]>]]"
                + "\n    -p:<PI>      Get the selected paragraph by index"
                + "\n    -l<LI>       Selects a link"
                + "\n    -s<[1|2]>    1: Shows page with links, 2: List just the page's links";
        }
    }
}

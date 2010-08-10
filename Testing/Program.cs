using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string[] tmp = Console.ReadLine().Split('|');
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://en.wikipedia.org/wiki/Special:Export/" + tmp[0]);
                webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                webRequest.UserAgent = ".NET Framework Example Client";
                webRequest.Accept = "application/xml";
                webRequest.Headers.Add("Accept-Charset", "utf-8");
                webRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    XmlReader xmlreader = new XmlTextReader(responseStream);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlreader);
                    xmlreader.Close();
                    webResponse.Close();

                    XmlNode node = doc.DocumentElement.GetElementsByTagName("text")[0];
                    if (node != null)
                        Console.WriteLine(node.InnerText);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while retrieve from Wikipedia. " + ex.ToString());
                }
                finally
                {

                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace IstHaltKeinBot
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Uri uri = new Uri(@"http://api.steampowered.com/ISteamApps/GetAppList/v0002/");
            WebRequest webRequest = WebRequest.Create(uri);
            WebResponse response = webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            String responseData = streamReader.ReadToEnd();
            var outObject = JsonConvert.DeserializeObject<RootObject>(responseData);

            int appNumber = outObject.applist.apps.Count;
            Random Rnd = new Random();
            int random1 = Rnd.Next(appNumber);
            int random2 = Rnd.Next(appNumber);

            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Tweet.PublishTweet(outObject.applist.apps[random1].name + " ist halt kein " + outObject.applist.apps[random2].name);
        }
    }

    public class App
    {
        public int appid { get; set; }
        public string name { get; set; }
    }

    public class Applist
    {
        public List<App> apps { get; set; }

    }

    public class RootObject
    {
        public Applist applist { get; set; }
    }

}

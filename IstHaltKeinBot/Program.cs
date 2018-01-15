using System;
using System.Collections.Generic;
using Tweetinvi;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace IstHaltKeinBot
{
    class Program
    {
        static void Main(string[] args)
        {
            String jsonfile = "steam.json";

            //If the json file is nonexistend or older than 7 days, it should be downloaded
            var threshold = DateTime.Now.AddDays(-7);
            var fileage = System.IO.File.GetLastWriteTime(jsonfile); //Age of the json file
            if (!File.Exists(jsonfile) || fileage  < threshold){
                //Downloads the latest json file of all games that exist on steam.
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile("http://api.steampowered.com/ISteamApps/GetAppList/v0002/", jsonfile);
            }

            //Reads the data from the steam.json
            StreamReader sr = new StreamReader(jsonfile);
            String jsonData = sr.ReadToEnd();
            var outObject = JsonConvert.DeserializeObject<RootObject>(jsonData);    //Serializes the JSon String
            sr.Close();

            //RNG generator - Is declared here, because if it is declared in generateRandomGame it does not work correctly...
            Random Rnd = new Random();

            //Writes a new tweet
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Tweet.PublishTweet(generateRandomGame(outObject.applist.apps, Rnd) + " ist halt kein " + generateRandomGame(outObject.applist.apps, Rnd) + ".");
        }

        
        private static string generateRandomGame(List<App> apps, Random Rnd)
        {
            int appNumber = apps.Count;   //How many Steam-Apps?

            String app = "";
            Regex r = new Regex("demo|dlc|soundtrack");

            do
            {
                app = apps[Rnd.Next(appNumber)].name;
            }
            while (r.IsMatch(app.ToLower()));

            return app;
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class URLCache
    {
        // number of characters we are going to use in the TinyURL
        private const int size = 7;
        // storage for URLs
        private Dictionary<string, string> urlCache;
        // initialize random variable
        private static Random random = new Random((int)DateTime.Now.Ticks);
        // key is TinyUrl and value is long Url 
        public URLCache()
        {
            urlCache = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetUrlCache()
        {
            return urlCache;
        }

        // function to create TinyUrl and return the result
        public string AddURL(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return "";
            }
            // checking if url starts with anything but http
            if (url.StartsWith("http") == false)
            {
                return "";
            }
            // checking if url is already created
            if (urlCache.ContainsValue(url.Trim()) == true)
            {
                return "TinyUrl is already created";
            }
            // perform in a loop for 1000 times until non duplicated TinyUrl is created. But getting a duplicated TynyUrl is almost impossible.
            // its going to the loop only if there is a duplicate
            for(int i=0; i<1000; i++)
            {
                string randomString = CreateRandomString();
                if (urlCache.ContainsKey(randomString) == false)
                {
                    string tinyUrl = String.Format("http://EvgeniaTinyUrl.com/{0}", randomString);
                    urlCache.Add(tinyUrl, url);
                    AppendToFile(tinyUrl, url);
                    return randomString;
                }
            }

            return "";
        }
        // adding urls to the database which is just a file  
        public void AppendToFile(string tinyURL, string URL)
        {
            string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string file = String.Format("{0}\\urlcache.txt", myDocuments);

            StreamWriter sw = new StreamWriter(new FileStream(file, FileMode.Append, FileAccess.Write), Encoding.UTF8);
            // write comma seperated line
            sw.WriteLine(String.Format("{0}  ,  {1}", tinyURL, URL)); 
            sw.Flush(); 
            sw.Close();
        }
        // load all created urls from the file to the memory(urlcache)
        public void LoadURLs()
        {
            string myDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string fileName = String.Format("{0}\\urlcache.txt", myDocuments);

            // read all the lines
            if (File.Exists(fileName))
            { 
                string[] lines = File.ReadAllLines(fileName);

                foreach(string line in lines)
                {
                    // parse and load
                    string[] items = line.Split(',');
                    if (items.Length == 2)
                    {
                        urlCache.Add(items[0].Trim(), items[1].Trim());
                    }
                }
            }
        }
        // creating a random string of lenght size(7)
        public string CreateRandomString()
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}

using System.Net;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheBestGameEver.Classes;

namespace TheBestGameEver
{
  class Program
  {
        static void Main(string[] args)
        {
            start_get();
        }
    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static void start_get()
    {
      HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://pokeapi.co/api/v2/pokemon/"));
      WebReq.Method = "GET";
      HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
      Console.WriteLine(WebResp.StatusCode);
      Console.WriteLine(WebResp.Server);
      string jsonString;
      using (Stream stream = WebResp.GetResponseStream()) //modified from your code since the using statement disposes the stream automatically when done
      {
          StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
          jsonString = reader.ReadToEnd();
      }
      Product items = JsonConvert.DeserializeObject<Product>(jsonString);
      Console.WriteLine(items.Count);     //returns 921, the number of items on that page
      Console.WriteLine(items.Next);
      if (items.Previous == null)
      {
        Console.WriteLine("Previous = null");
      }
      else
      {
        Console.WriteLine(items.Previous);
      }
      foreach (var result in items.Results)
      {
        if (result == null)
        {
          Console.WriteLine("results = null");
        }
        Console.WriteLine(result.Name);
        Console.WriteLine(result.Url);
      }
    }
   
  }
}

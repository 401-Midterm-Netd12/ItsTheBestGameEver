using System.Net;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheBestGameEver.Classes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TheBestGameEver
{
  class Program
  {
    static HttpClient client = new HttpClient();
    static string URL = "https://asyncinnapp.azurewebsites.net/";
    static void Main(string[] args)
    {
      RunAsync();
      start_get();

    }
    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static void start_get()
    {
      HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format(URL + "api/Amenities"));
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
      List<AmenitiesObject> items = JsonConvert.DeserializeObject<List<AmenitiesObject>>(jsonString);
      foreach (var amenity in items)
      {
        Console.WriteLine(amenity.ID);
        Console.WriteLine(amenity.Item);
      }
    }
    static async Task<Uri> CreateProductAsync(AmenitiesObject amenity)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Amenities", amenity);
      response.EnsureSuccessStatusCode();
      return response.Headers.Location;
    }
    static async void RunAsync()
    {
      // Update port # in the following line.
      client.BaseAddress = new Uri(URL);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));
      try
      {
        // Create a new product
        AmenitiesObject amenity = new AmenitiesObject
        {
          Item = "Gizmo"
        };
        var url = await CreateProductAsync(amenity);
        Console.WriteLine($"Created at {url}");
      }
      catch(Exception e)
      {
        throw e;
      }
      
        
    }
  }
}

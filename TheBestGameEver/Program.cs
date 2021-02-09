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
    static HttpWebRequest WebReq;
    static HttpWebResponse WebResp;
    static void Main(string[] args)
    {
      start_get();
      RunAsync();
      start_get();

    }
    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static List<AmenitiesObject> start_get(int id = 0)
    {
      string newURL = $"{URL}api/Amenities/";
      if (id != 0)
      {
        newURL += id;
        Console.WriteLine(newURL);
      }
      WebReq = (HttpWebRequest)WebRequest.Create(string.Format(newURL));
      WebReq.Method = "GET";
      WebResp = (HttpWebResponse)WebReq.GetResponse();
      Console.WriteLine(WebResp.StatusCode);
      Console.WriteLine(WebResp.Server);
      string jsonString;
      using (Stream stream = WebResp.GetResponseStream()) //modified from your code since the using statement disposes the stream automatically when done
      {
        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        jsonString = reader.ReadToEnd();
      }
      if (id == 0)
      {
        List<AmenitiesObject> items = JsonConvert.DeserializeObject<List<AmenitiesObject>>(jsonString);
        foreach (var amenity in items)
        {
          Console.WriteLine(amenity.ID);
          Console.WriteLine(amenity.Item);
        }
        return items;
      }
      List<AmenitiesObject> itemsA = new List<AmenitiesObject>();
      itemsA.Add(JsonConvert.DeserializeObject<AmenitiesObject>(jsonString));
      return itemsA;
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
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      try
      {
        // Create a new product
        AmenitiesObject amenity = new AmenitiesObject
        {
          Item = "Balcony"
        };
        // var url = await CreateProductAsync(amenity);
        // Console.WriteLine($"Created at {url}");
        //List<AmenitiesObject> updateAmenity = start_get(6);
        //foreach (AmenitiesObject item in updateAmenity)
        //{
        //  Console.WriteLine(item.ID);
        //  Console.WriteLine(item.Item);
        //  item.Item = "Banana";
        //  Console.WriteLine(item.ID);
        //  Console.WriteLine(item.Item);
        //  await UpdateProductAsync(item);
        //}
        Console.WriteLine("Starting delete method");
        string DelResp = DeleteProduct(9);
        Console.WriteLine(DelResp);
      }
      catch (Exception e)
      {
        throw e;
      }

    }
    static async Task<AmenitiesObject> UpdateProductAsync(AmenitiesObject amenity)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Amenities/{amenity.ID}", amenity);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      amenity = await response.Content.ReadAsAsync<AmenitiesObject>();
      return amenity;
    }
    static string DeleteProduct(int id)
    {
      Console.WriteLine("inside Delete Function");
      string StrResp;
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Amenities/{id}");
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();
      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
        Console.WriteLine("inside StreamReader");
      }
      return StrResp;
    }
  }
}


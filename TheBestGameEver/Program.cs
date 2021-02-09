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
    static string URL = "https://customcharacter1.azurewebsites.net/";
    static HttpWebRequest WebReq;
    static HttpWebResponse WebResp;
    static Abilities ability = new Abilities { ID = 4, Name = "Fire Ball", Description = "Really cool" };
    static void Main(string[] args)
    {
      start_get();
      // RunAsync();
      start_get();

    }
    //private static bool MainMenu()
    //{
    //  Console.Clear();
    //  Console.WriteLine("Welcome traveler, please choose an option!");
    //  Console.WriteLine("1) Create a character");
    //  Console.WriteLine("2) Create a skill");
    //  Console.WriteLine("3) Create an ability");
    //  Console.WriteLine("4) View all races");
    //  Console.WriteLine("5) View all classes");
    //  Console.WriteLine("6) View all skills");
    //  Console.WriteLine("7) View all abilities");
    //  Console.WriteLine("8) Update your character");
    //  Console.WriteLine("9) Update your abilities");
    //  Console.WriteLine("10) Update your skills");
    //  Console.WriteLine("11) Delete your character");

    //  switch (Console.ReadLine())
    //  {
    //    case "1":
    //      start_get();
    //      return true;
    //   // case "2":

    //  }


    //}


    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static List<Abilities> start_get(int id = 0)
    {
      string newURL = $"{URL}api/Abilities/";
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
        List<Abilities> items = JsonConvert.DeserializeObject<List<Abilities>>(jsonString);
        foreach (var ability in items)
        {
          Console.WriteLine(ability.ID);
          Console.WriteLine(ability.Name);
          Console.WriteLine(ability.Description);
        }
        return items;
      }
      List<Abilities> itemsA = new List<Abilities>();
      itemsA.Add(JsonConvert.DeserializeObject<Abilities>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateProductAsync(Abilities ability)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Abilities", ability);
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
        // Create a new ability
        Abilities ability = new Abilities
        {
          Name = "Throwing snowballs"
        };
        var url = await CreateProductAsync(ability);
        Console.WriteLine($"Created at {url}");

        // Update an ability
        //List<Abilities> updateAbility = start_get(3);
        //foreach (Abilities item in updateAbility)
        //{
        //  Console.WriteLine(item.ID);
        //  Console.WriteLine(item.Name);
        //  Console.WriteLine(item.Description);
        //  item.Name = "Fire throwing";
        //  Console.WriteLine(item.ID);
        //  Console.WriteLine(item.Name);
        //  Console.WriteLine(item.Description);
        //  await UpdateProductAsync(item);
        //}

        // Delete an ability
        //Console.WriteLine("Starting delete method");
        //string DelResp = DeleteProduct(5);
        //Console.WriteLine(DelResp);
      }
      catch (Exception e)
      {
        throw e;
      }

    }
    static async Task<Abilities> UpdateProductAsync(Abilities ability)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Abilities/{ability.ID}", ability);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      ability = await response.Content.ReadAsAsync<Abilities>();
      return ability;
    }
    static string DeleteProduct(int id)
    {
      Console.WriteLine("inside Delete Function");
      string StrResp;
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Abilities/{id}");
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


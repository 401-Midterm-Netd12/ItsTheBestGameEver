using System.Net;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheBestGameEver.Classes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using TheBestGameEver.Models;

namespace TheBestGameEver
{
  class Program
  {
    static HttpClient client = new HttpClient();
    static string URL = "https://customcharacter1.azurewebsites.net/";
    static HttpWebRequest WebReq;
    static HttpWebResponse WebResp;
    static void Main(string[] args)
    {
      client.BaseAddress = new Uri(URL);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      //read();
      //create();
      //read();
      //delete();
      //read();
      //update();
      //read();
      ReadSkill();
      CreateSkill();
      ReadSkill();

    }

    static async void read()
    {
      await start_get();
    }

    static async void create()
    {
      Ability ability = new Ability
      {
        Name = "Night vision",
        Desc = "Can see... at night.."

      };
      var url = await CreateAbilityAsync(ability);
      Console.WriteLine($"Created at {url}");
    }

    static async void delete()
    {
      Console.WriteLine("Starting delete method");
      string DelResp = await DeleteAbility(3);
    }

    static async void update()
    {
      List<Ability> updateAbility = await start_get(2);
      Ability ability = updateAbility.First();
      ability.Name = "Fireball casting";
      ability.Desc = "Things get real hot";
      await UpdateAbilityAsync(ability);
    }

    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static async Task<List<Ability>> start_get(int id = 0)
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
        List<Ability> items = JsonConvert.DeserializeObject<List<Ability>>(jsonString);
        foreach (var ability in items)
        {
          Console.WriteLine(ability.ID);
          Console.WriteLine(ability.Name);
        }
        return items;
      }
      List<Ability> itemsA = new List<Ability>();
      itemsA.Add(JsonConvert.DeserializeObject<Ability>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateAbilityAsync(Ability ability)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Abilities", ability);
      response.EnsureSuccessStatusCode();
      return response.Headers.Location;
    }

    static async Task<Ability> UpdateAbilityAsync(Ability ability)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Abilities/{ability.ID}", ability);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      ability = await response.Content.ReadAsAsync<Ability>();
      return ability;
    }
    static async Task<string> DeleteAbility(int id)
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
    // These are going to be our skills methods below
    




    static async void ReadSkill()
    {
      await start_getSkill();
    }

    static async void CreateSkill()
    {
      Skill skill = new Skill
      {
        Name = "Dual Wielding",
        Desc = "Can hold not one, but two things"
      };
      var url = await CreateSkillAsync(skill);
      Console.WriteLine($"Created at {url}");
    }

    static async void DeleteSkill()
    {
      Console.WriteLine("Starting delete method");
      string DelResp = await DeleteSkill(1);
    }

    static async void UpdateSkill()
    {
      List<Skill> updateSkill = await start_getSkill(7);
      Skill skill = updateSkill.First();
      skill.Name = "Built in emotional support cat";
      await UpdateSkillAsync(skill);
    }

    private static async Task<List<Skill>> start_getSkill(int id = 0)
    {
      string newURL = $"{URL}api/Skills/";
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
        List<Skill> items = JsonConvert.DeserializeObject<List<Skill>>(jsonString);
        foreach (var skill in items)
        {
          Console.WriteLine(skill.ID);
          Console.WriteLine(skill.Name);
        }
        return items;
      }
      List<Skill> itemsA = new List<Skill>();
      itemsA.Add(JsonConvert.DeserializeObject<Skill>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateSkillAsync(Skill skill)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Skills", skill);
      response.EnsureSuccessStatusCode();
      return response.Headers.Location;
    }

    static async Task<Skill> UpdateSkillAsync(Skill skill)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Skills/{skill.ID}", skill);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      skill = await response.Content.ReadAsAsync<Skill>();
      return skill;
    }
    static async Task<string> DeleteSkill(int id)
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


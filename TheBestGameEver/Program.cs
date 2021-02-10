﻿using System.Net;
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
using TheBestGameEver.Models.DTOs;

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

      //ReadSkill();
      // CreateSkill();
      // ReadSkill();
      // UpdateSkill();
      //ReadSkill();
      //DeleteSkill();
      //ReadSkill();

      // ReadClass();
      // CreateClass();
      //ReadClass();
      //DeleteClass();
      //ReadClass();
      //UpdateClass();
      //ReadClass();

      // ReadRace();
      // CreateRace();
      //ReadRace();
      //UpdateRace();
      //ReadRace();
      //DeleteRace();
      //ReadRace();

      ReadCharacter();
      CreateCharacter();
      ReadCharacter();
      //UpdateCharacter();
      //ReadCharacter();


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
      List<Ability> updateAbility = await start_get(5);
      Ability ability = updateAbility.First();
      ability.Name = "Built in emotional support cat";
      ability.Desc = "You can cry into a soft object";
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
      try
      {
        HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Abilities/{ability.ID}", ability);
        response.EnsureSuccessStatusCode();

        // Deserialize the updated product from the response body.
        ability = await response.Content.ReadAsAsync<Ability>();
        return ability;
      }
      catch(Exception e)
      {
        Console.WriteLine(e);
        throw e;
      }
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
      string DelResp = await DeleteSkill(7);
    }

    static async void UpdateSkill()
    {
      List<Skill> updateSkill = await start_getSkill(7);
      Skill skill = updateSkill.First();
      skill.ID = 7;
      skill.Name = "Built in emotional support cat";
      skill.Desc = "You can cry into a soft object";
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
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Skills/{id}");
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();
      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
        Console.WriteLine("inside StreamReader");
      }
      return StrResp;
    }


    // methods for classes 
    static async void ReadClass()
    {
      await start_getClass();
    }

    static async void CreateClass()
    {
      Class charClass = new Class
      {
        ClassName = "Knight",
        StatModifier = 5
      };
      var url = await CreateClassAsync(charClass);
      Console.WriteLine($"Created at {url}");
    }

    static async void DeleteClass()
    {
      Console.WriteLine("Starting delete method");
      string DelResp = await DeleteClass(4);
    }

    static async void UpdateClass()
    {
      List<Class> updateClass = await start_getClass(3);
      Class charClass = updateClass.First();
      charClass.ID = 3;
      charClass.StatModifier = 4;
      charClass.ClassName = "Throwing cats at enemies";
      await UpdateClassAsync(charClass);
    }

    private static async Task<List<Class>> start_getClass(int id = 0)
    {
      string newURL = $"{URL}api/Classes/";
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
        List<Class> items = JsonConvert.DeserializeObject<List<Class>>(jsonString);
        foreach (var charClass in items)
        {
          Console.WriteLine(charClass.ID);
          Console.WriteLine(charClass.ClassName);
        }
        return items;
      }
      List<Class> itemsA = new List<Class>();
      itemsA.Add(JsonConvert.DeserializeObject<Class>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateClassAsync(Class charClass)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Classes/", charClass);
      response.EnsureSuccessStatusCode();
      return response.Headers.Location;
    }

    static async Task<Class> UpdateClassAsync(Class charClass)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Classes/{charClass.ID}", charClass);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      charClass = await response.Content.ReadAsAsync<Class>();
      return charClass;
    }
    static async Task<string> DeleteClass(int id)
    {
      Console.WriteLine("inside Delete Function");
      string StrResp;
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Classes/{id}");
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();
      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
        Console.WriteLine("inside StreamReader");
      }
      return StrResp;
    }

    // methods for race
    static async void ReadRace()
    {
      await start_getRace();
    }

    static async void CreateRace()
    {
      Race race = new Race
      {
        RaceType = "Elf",
        StatModifier = 5
      };
      var url = await CreateRaceAsync(race);
      Console.WriteLine($"Created at {url}");
    }

    static async void DeleteRace()
    {
      Console.WriteLine("Starting delete method");
      string DelResp = await DeleteRace(5);
    }

    static async void UpdateRace()
    {
      List<Race> updateRace = await start_getRace(3);
      Race race = updateRace.First();
      race.ID = 3;
      race.StatModifier = 4;
      race.RaceType = "Throwing cats at enemies";
      await UpdateRaceAsync(race);
    }

    private static async Task<List<Race>> start_getRace(int id = 0)
    {
      string newURL = $"{URL}api/Races/";
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
        List<Race> items = JsonConvert.DeserializeObject<List<Race>>(jsonString);
        foreach (var race in items)
        {
          Console.WriteLine(race.ID);
          Console.WriteLine(race.RaceType);
          Console.WriteLine(race.StatModifier);
        }
        return items;
      }
      List<Race> itemsA = new List<Race>();
      itemsA.Add(JsonConvert.DeserializeObject<Race>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateRaceAsync(Race race)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Races/", race);
      Console.WriteLine(response.Headers.Location.ToString());
      response.EnsureSuccessStatusCode();
      Console.WriteLine(response.Headers.Location.ToString());
      return response.Headers.Location;
    }

    static async Task<Race> UpdateRaceAsync(Race race)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Races/{race.ID}", race);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      race = await response.Content.ReadAsAsync<Race>();
      return race;
    }
    static async Task<string> DeleteRace(int id)
    {
      Console.WriteLine("inside Delete Function");
      string StrResp;
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Races/{id}");
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();
      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
        Console.WriteLine("inside StreamReader");
      }
      return StrResp;
    }


    // methods for character

    static async void ReadCharacter()
    {
      await start_getRace();
    }

    static async void CreateCharacter()
    {
      Character character = new Character
      {
        UserId = "",
        RaceId = 3,
        ClassId = 3,
        Name = "John",
        HP = 10,
        Dex = 1,
        Strength = 8
      };
      var url = await CreateCharacterAsync(character);
      Console.WriteLine($"Created at {url}");
    }

    static async void DeleteCharacter()
    {
      Console.WriteLine("Starting delete method");
      string DelResp = await DeleteCharacter(5);
    }

    static async void UpdateCharacter()
    {
      List<Character> updateCharacter = await start_getCharacter(2);
      Character character = updateCharacter.First();
      character.UserId = "JohnIsCool";
      character.RaceId = 3;
      character.ClassId = 3;
      character.Name = "John";
      character.HP = 10;
      character.Dex = 1;
      character.Strength = 8;
      await UpdateCharacterAsync(character);
    }

    private static async Task<List<Character>> start_getCharacter(int id = 0)
    {
      string newURL = $"{URL}api/Character/";
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
        List<Character> items = JsonConvert.DeserializeObject<List<Character>>(jsonString);
        foreach (var character in items)
        {
          Console.WriteLine(character.RaceId);
          Console.WriteLine(character.ClassId);
          Console.WriteLine(character.Name);
          Console.WriteLine(character.HP);
          Console.WriteLine(character.Dex);
          Console.WriteLine(character.Strength);
        }
        return items;
      }
      List<Character> itemsA = new List<Character>();
      itemsA.Add(JsonConvert.DeserializeObject<Character>(jsonString));
      return itemsA;
    }
    static async Task<Uri> CreateCharacterAsync(Character character)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api/Character/", character);
      Console.WriteLine(response.Headers.Location.ToString());
      response.EnsureSuccessStatusCode();
      Console.WriteLine(response.Headers.Location.ToString());
      return response.Headers.Location;
    }

    static async Task<Character> UpdateCharacterAsync(Character character)
    {
      HttpResponseMessage response = await client.PutAsJsonAsync(
          $"api/Character/{character.Id}", character);
      response.EnsureSuccessStatusCode();

      // Deserialize the updated product from the response body.
      character = await response.Content.ReadAsAsync<Character>();
      return character;
    }
    static async Task<string> DeleteCharacter(int id)
    {
      Console.WriteLine("inside Delete Function");
      string StrResp;
      WebReq = (HttpWebRequest)WebRequest.Create(URL + $"api/Character/{id}");
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


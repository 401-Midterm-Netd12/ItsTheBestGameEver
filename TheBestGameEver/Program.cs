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
using TheBestGameEver.Models.DTOs;
using System.Text;

namespace TheBestGameEver
{
  class Program
  {
    static HttpClient client = new HttpClient();
    static string URL = "https://customcharacter1.azurewebsites.net";
    static HttpWebRequest WebReq;
    static HttpWebResponse WebResp;
    static void Main(string[] args)
    {
      client.BaseAddress = new Uri(URL);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      read();
      //delete();

      //create();
      //update();

      // CreateSkill();
      // UpdateSkill();
      //DeleteSkill();

      // CreateClass();
      //DeleteClass();
      //UpdateClass();

      // CreateRace();
      //UpdateRace();
      //DeleteRace();

      //CreateCharacter();
      //UpdateCharacter();


      /* === Menu Commands === */

      //Console.ForegroundColor = ConsoleColor.White;
      //LoginMenu();
      //CreateACharacterMenu();

    }

    static async void read()
    {
      await GetModels(CurrentURL(CharacterModels.Ability));
      await GetModels(CurrentURL(CharacterModels.Race));
      await GetModels(CurrentURL(CharacterModels.Skill));
      await GetModels(CurrentURL(CharacterModels.Class));
      await GetModels(CurrentURL(CharacterModels.Character));

      await GetModels(CurrentURL(CharacterModels.Ability), 1);
      await GetModels(CurrentURL(CharacterModels.Race), 1);
      await GetModels(CurrentURL(CharacterModels.Skill), 4);
      await GetModels(CurrentURL(CharacterModels.Class), 3);
      await GetModels(CurrentURL(CharacterModels.Character), 2);
    }

    static async void delete()
    {
      string DelResp;
      Console.WriteLine("Starting delete method");
      DelResp = await DeleteModels(4, CurrentURL(CharacterModels.Character));
      DelResp = await DeleteModels(5, CurrentURL(CharacterModels.Ability));
      DelResp = await DeleteModels(1, CurrentURL(CharacterModels.Class));
      DelResp = await DeleteModels(3, CurrentURL(CharacterModels.Race));
      DelResp = await DeleteModels(6, CurrentURL(CharacterModels.Skill));
    }






    static async void create()
    {
      Ability ability = new Ability
      {
        name = "Night vision",
        desc = "Can see... at night.."
      };

      var Result = CreateTestCode(ability);
      Console.WriteLine(Result.ToString());
    }

    //static async void update()
    //{
    //  List<Ability> updateAbility = await start_get(5);
    //  Ability ability = updateAbility.First();
    //  ability.Name = "Built in emotional support cat";
    //  ability.Desc = "You can cry into a soft object";
    //  await UpdateAbilityAsync(ability);
    //}

    /* ==================================================================================================== */
    /* ========================================== Helper Methods ========================================== */
    /* ==================================================================================================== */

    static string CurrentURL(CharacterModels currentModel)
    {
      switch (currentModel)
      {
        case CharacterModels.Ability:
          return "/api/Abilities";
        case CharacterModels.Class:
          return "/api/Classes";
        case CharacterModels.Race:
          return "/api/Races";
        case CharacterModels.Skill:
          return "/api/Skills";
        case CharacterModels.Character:
          return "/api/Character";
        default:
          return "";
      }
    }

    /* ==================================================================================================== */
    /* =========================================== CRUD Methods =========================================== */
    /* ==================================================================================================== */


    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    private static async Task<List<Ability>> GetModels(string modelURL, int id = 0)
    {
      string newURL = URL + modelURL;
      string jsonString;

      if (id != 0)
      {
        newURL += id;
      }

      WebReq = (HttpWebRequest)WebRequest.Create(string.Format(newURL));
      WebReq.Method = "GET";
      WebResp = (HttpWebResponse)WebReq.GetResponse();

      using (Stream stream = WebResp.GetResponseStream()) //modified from your code since the using statement disposes the stream automatically when done
      {
        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        jsonString = reader.ReadToEnd();
      }

      if (id == 0)
      {
        var items = JsonConvert.DeserializeObject<List<Ability>>(jsonString);
        return items;
      }
      List<Ability> itemsA = new List<Ability>();
      itemsA.Add(JsonConvert.DeserializeObject<Ability>(jsonString));
      Console.WriteLine(itemsA);
      return itemsA;
    }

    static async Task<string> DeleteModels(int id, string modelURL)
    {
      string StrResp;
      string newURL = URL + modelURL + id;

      WebReq = (HttpWebRequest)WebRequest.Create(newURL);
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();

      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
      }

      return StrResp;
    }












    //static void CreateTestCode(Ability newAbility)
    //{
    //  string newURL = URL + "/api/Abilities";
    //  JsonSerializer serializer = new JsonSerializer();
    //  string jsonObj = JsonConvert.SerializeObject(newAbility);

    //  WebReq = (HttpWebRequest)WebRequest.Create(string.Format(URL));
    //  WebReq.ContentType = "application/json; charset=utf-8";
    //  WebReq.Method = "POST";
    //  WebReq.Accept = "application/json; charset=utf-8";

    //  Console.WriteLine(jsonObj);

    //  using (StreamWriter stream = new StreamWriter(WebReq.GetRequestStream()))
    //  {
    //    stream.Write(jsonObj);
    //  }
    //}


    static HttpResponseMessage CreateTestCode(Ability newAbility)
    {
      HttpResponseMessage response = null;
      string jsonObj = JsonConvert.SerializeObject(newAbility);
      Console.WriteLine(jsonObj);
      try
      {
        response = client.PostAsync("/api/Abilities", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
        response.EnsureSuccessStatusCode();
        // Handle success
      }
      catch (HttpRequestException)
      {
        Console.WriteLine("Code broke");
        // Handle failure
      }
      return response;
    }




    //static async Task<Uri> CreateModels(string modelURL, Character characterObj = null, Ability abilityObj = null, Race raceObj = null, Skill skillObj = null, Class classObj = null)
    //{
    //  string newURL = URL + modelURL;
    //  HttpResponseMessage response = null;

    //  if (abilityObj != null)
    //  {
    //    response = await client.PostAsJsonAsync(
    //        newURL, abilityObj);
    //  }
    //  else if(characterObj != null)
    //  {
    //    response = await client.PostAsJsonAsync(
    //        newURL, characterObj);
    //  }
    //  else if (skillObj != null)
    //  {
    //    response = await client.PostAsJsonAsync(
    //        newURL, skillObj);
    //  }
    //  else if (raceObj != null)
    //  {
    //    response = await client.PostAsJsonAsync(
    //        newURL, raceObj);
    //  }
    //  else if (classObj != null)
    //  {
    //    response = await client.PostAsJsonAsync(
    //        newURL, classObj);
    //  }
    //  else
    //  {

    //  }

    //  response.EnsureSuccessStatusCode();
    //  return response.Headers.Location;
    //}

    //static async Task<Ability> UpdateAbilityAsync(Ability ability)
    //{
    //  try
    //  {
    //    HttpResponseMessage response = await client.PutAsJsonAsync(
    //      $"api/Abilities/{ability.id}", ability);
    //    response.EnsureSuccessStatusCode();

    //    // Deserialize the updated product from the response body.
    //    ability = await response.Content.ReadAsAsync<Ability>();
    //    return ability;
    //  }
    //  catch(Exception e)
    //  {
    //    Console.WriteLine(e);
    //    throw e;
    //  }
    //}
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


    /* ==================================================================================================== */
    /* =========================================== Menu Methods =========================================== */
    /* ==================================================================================================== */

    static void ConsoleHeader()
    {
      Console.WriteLine("\t\t\t============================================================================");
      Console.WriteLine("\t\t\t= ||\\    /||   ||\\    ||                      //////////////               =");
      Console.WriteLine("\t\t\t= || \\  / ||   || \\   ||                   ////           ////             =");
      Console.WriteLine("\t\t\t= ||  \\/  ||   ||  \\  ||                ////                ////           =");
      Console.WriteLine("\t\t\t= ||      ||   ||   \\ ||              ////                   ////          =");
      Console.WriteLine("\t\t\t= ||      || o ||    \\|| o            ==========|     |==========          =");
      Console.WriteLine("\t\t\t=                                  =====|  O    |=====|  O    |====        =");
      Console.WriteLine("\t\t\t=                                   @@@ ========|     |======== @@         =");
      Console.WriteLine("\t\t\t=                                    @                          @          =");
      Console.WriteLine("\t\t\t=                                    @@@      |---------|      @@.         =");
      Console.WriteLine("\t\t\t=                                       @@@   |---------|  *@@#            =");
      Console.WriteLine("\t\t\t=                                          %@@@@@////#@@@@@.               =");
      Console.WriteLine("\t\t\t=                                             /////////                    =");
      Console.WriteLine("\t\t\t============================================================================");
      PushScreenDown(2);
    }

    static void ScreenHeaderDisplay()
    {
      Console.Clear();
      PushScreenDown(2);
      ConsoleHeader();
    }

    static void PauseScreen()
    {
      Console.WriteLine("\n\nPlease press enter to continue...");
      Console.ReadKey();
    }

    static void PushScreenDown(int lines)
    {
      for (int i = 0; i < lines; i++)
      {
        Console.WriteLine();
      }
    }

    // Answer From: https://stackoverflow.com/questions/21917203/how-do-i-center-text-in-a-console-application
    // Answer By: EZI
    // Answer On: Feb 20 '14 at 19:29
    static void CenterConsoleText(int stringLength)
    {
      Console.SetCursorPosition((Console.WindowWidth - stringLength) / 2, Console.CursorTop);
    }

    static void MakeSelection(int maxChoices)
    {
      Console.ForegroundColor = ConsoleColor.White;
      CenterConsoleText(35);
      Console.Write("Please Make a Selection ");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("(1-{0}): ", maxChoices);
      Console.ForegroundColor = ConsoleColor.White;
    }

    static void InvalidSelection()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("\nInvalid Selection...");
      Console.ForegroundColor = ConsoleColor.White;
      PauseScreen();
    }

    static void LoginMenu()
    {
      bool exit = false;
      string userInput;

      do
      {
        ScreenHeaderDisplay();
        CenterConsoleText(35);
        Console.Write("Welcome to ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("It's the Best Game Ever!\n");
        Console.ForegroundColor = ConsoleColor.Green;
        CenterConsoleText(35);
        Console.WriteLine("\t1. Login");
        CenterConsoleText(35);
        Console.WriteLine("\t2. Register");
        CenterConsoleText(35);
        Console.WriteLine("\t3. Exit\n");
        Console.ForegroundColor = ConsoleColor.White;
        MakeSelection(3);
        userInput = Console.ReadLine();

        switch (userInput)
        {
          case "1":
            UserLogin();
            exit = true;
            break;
          case "2":
            RegisterUser();
            break;
          case "3":
            exit = true;
            break;
          default:
            InvalidSelection();
            break;
        }

        Console.ForegroundColor = ConsoleColor.White;
      } while (!exit);
    }

    // Answer From: https://stackoverflow.com/questions/23433980/c-sharp-console-hide-the-input-from-console-window-while-typing
    // Answer By: dataCore
    // Answer On: Mar 31 '16 at 11:41
    static string getPasswd()
    {
      string userPwd = "";

      while (true)
      {
        var key = System.Console.ReadKey(true);
        if (key.Key == ConsoleKey.Enter)
        {
          break;
        }
        userPwd += key.KeyChar;
      }

      return userPwd;
    }

    static void RegisterUser()
    {
      bool exit = false;
      string user;
      string userPwd = "";
      string confirmUserPwd = "";
      string userEmail;

      do
      {
        ScreenHeaderDisplay();
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.WriteLine("Please enter your information: \n");
        Console.ForegroundColor = ConsoleColor.Green;
        CenterConsoleText(35);
        Console.Write("User Name: ");
        Console.ForegroundColor = ConsoleColor.White;
        user = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        CenterConsoleText(35);
        Console.Write("Password: ");
        Console.ForegroundColor = ConsoleColor.White;
        userPwd = getPasswd();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        CenterConsoleText(35);
        Console.Write("Confirm Password: ");
        Console.ForegroundColor = ConsoleColor.White;
        confirmUserPwd = getPasswd();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        CenterConsoleText(35);
        Console.Write("Email: ");
        Console.ForegroundColor = ConsoleColor.White;
        userEmail = Console.ReadLine();

        if (userPwd == confirmUserPwd)
        {
          Console.ForegroundColor = ConsoleColor.DarkCyan;
          PushScreenDown(2);
          CenterConsoleText(35);
          Console.WriteLine("Please return to main menu and login");
          Console.ForegroundColor = ConsoleColor.White;
          PauseScreen();
          exit = true;
        }
        else
        {
          Console.WriteLine("\nPasswords do not match...");
          PauseScreen();
        }
      } while (!exit);
    }

    static void UserLogin()
    {
      bool exit = false;
      string user;
      string userPwd = "";

      do
      {
        ScreenHeaderDisplay();
        Console.ForegroundColor = ConsoleColor.Green;
        CenterConsoleText(35);
        Console.Write("Login: ");
        Console.ForegroundColor = ConsoleColor.White;
        user = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        CenterConsoleText(35);
        Console.Write("Password: ");
        Console.ForegroundColor = ConsoleColor.White;
        userPwd = getPasswd();
        exit = true;
      } while (!exit);
    }

    static void CreateACharacterMenu()
    {
      bool exit = false;
      string userChoice;

      do
      {
        ScreenHeaderDisplay();
        CenterConsoleText(35);
        Console.WriteLine("What would you like to do?\n");
        CenterConsoleText(35);
        Console.Write("1. Create/Modify a");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Character");
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.Write("2. Create/Modify a");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" Race");
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.Write("3. Create/Modify a");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" Class");
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.Write("4. Create/Modify a");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(" Ability");
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.Write("5. Create/Modify a");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" Skill");
        Console.ForegroundColor = ConsoleColor.White;
        CenterConsoleText(35);
        Console.WriteLine("6. Exit");
        PushScreenDown(1);
        MakeSelection(6);
        userChoice = Console.ReadLine();

        switch (userChoice)
        {
          case "1":
            break;
          case "2":
            break;
          case "3":
            break;
          case "4":
            break;
          case "5":
            break;
          case "6":
            exit = true;
            break;
          default:
            InvalidSelection();
            break;
        }
      } while (!exit);
    }


  }//end of class Program

  public enum CharacterModels
  {
    Character,
    Race,
    Class,
    Skill,
    Ability
  }
}// end of namespace


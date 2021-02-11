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

      //read();
      //delete();

      //create();
      update();

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
      CRUD<Ability> abilityObj = new CRUD<Ability>();
      List<Ability> newAbilityList = new List<Ability>();
      newAbilityList = await abilityObj.GetModels(CurrentURL(CharacterModels.Ability));
      Console.WriteLine(newAbilityList.First().id);
      Console.WriteLine(newAbilityList.First().name);
      Console.WriteLine(newAbilityList.First().desc);
      newAbilityList = await abilityObj.GetModels(CurrentURL(CharacterModels.Ability), 1);
      Console.WriteLine(newAbilityList.First().id);
      Console.WriteLine(newAbilityList.First().name);
      Console.WriteLine(newAbilityList.First().desc);

      CRUD<Race> raceObj = new CRUD<Race>();
      List<Race> newRaceList = new List<Race>();
      newRaceList = await raceObj.GetModels(CurrentURL(CharacterModels.Race));
      Console.WriteLine(newRaceList.First().ID);
      Console.WriteLine(newRaceList.First().RaceType);
      Console.WriteLine(newRaceList.First().StatModifier);
      newRaceList = await raceObj.GetModels(CurrentURL(CharacterModels.Race), 1);
      Console.WriteLine(newRaceList.First().ID);
      Console.WriteLine(newRaceList.First().RaceType);
      Console.WriteLine(newRaceList.First().StatModifier);

      CRUD<Skill> skillObj = new CRUD<Skill>();
      List<Skill> newSkillList = new List<Skill>();
      newSkillList = await skillObj.GetModels(CurrentURL(CharacterModels.Skill));
      Console.WriteLine(newSkillList.First().ID);
      Console.WriteLine(newSkillList.First().Name);
      Console.WriteLine(newSkillList.First().Desc);
      newSkillList = await skillObj.GetModels(CurrentURL(CharacterModels.Skill), 1);
      Console.WriteLine(newSkillList.First().ID);
      Console.WriteLine(newSkillList.First().Name);
      Console.WriteLine(newSkillList.First().Desc);

      CRUD<Class> classObj = new CRUD<Class>();
      List<Class> newClassList = new List<Class>();
      newClassList = await classObj.GetModels(CurrentURL(CharacterModels.Class));
      Console.WriteLine(newClassList.First().ID);
      Console.WriteLine(newClassList.First().ClassName);
      Console.WriteLine(newClassList.First().StatModifier);
      newClassList = await classObj.GetModels(CurrentURL(CharacterModels.Class), 1);
      Console.WriteLine(newClassList.First().ID);
      Console.WriteLine(newClassList.First().ClassName);
      Console.WriteLine(newClassList.First().StatModifier);

      CRUD<Character> characterObj = new CRUD<Character>();
      List<Character> newCharacterList = new List<Character>();
      newCharacterList = await characterObj.GetModels(CurrentURL(CharacterModels.Character));
      Console.WriteLine(newCharacterList.First().Id);
      Console.WriteLine(newCharacterList.First().Name);
      Console.WriteLine(newCharacterList.First().UserId);
      newCharacterList = await characterObj.GetModels(CurrentURL(CharacterModels.Character), 1);
      Console.WriteLine(newCharacterList.First().Id);
      Console.WriteLine(newCharacterList.First().Name);
      Console.WriteLine(newCharacterList.First().UserId);
    }

    static async void delete()
    {
      CRUD<string> delObj = new CRUD<string>();
      Console.WriteLine(await delObj.DeleteModels(6, CurrentURL(CharacterModels.Character)));
      Console.WriteLine(await delObj.DeleteModels(1, CurrentURL(CharacterModels.Race)));
      Console.WriteLine(await delObj.DeleteModels(4, CurrentURL(CharacterModels.Class)));
      Console.WriteLine(await delObj.DeleteModels(2, CurrentURL(CharacterModels.Skill)));
      Console.WriteLine(await delObj.DeleteModels(4, CurrentURL(CharacterModels.Ability)));
    }

    static async void create()
    {
      Ability ability1 = new Ability()
      {
          name = "Thermal Vision",
          desc = "Can see your heat"
      };
      Ability ability2 = new Ability()
      {
          name = "Charm",
          desc = "High Charisma Stat"
      };
      Skill skill1 = new Skill()
      {
        Name = "Super Speed",
        Desc = "I go fast"
      };
      Character character = new Character()
      {
          UserId = "lkwuefhwd",
          RaceId = 2,
          ClassId = 3,
          Name = "Bob Belcher",
          HP = 12,
          Dex = 10,
          Strength = 3
      };
      Race race = new Race()
      {
        RaceType = "Dwarf",
        StatModifier = 10
      };
      Class newClass = new Class()
      {
        StatModifier = 5,
        ClassName = "Wood Elf"

      };
      CRUD<Ability> abilityObj = new CRUD<Ability>();
      abilityObj.CreateModel(ability1, CurrentURL(CharacterModels.Ability));
      abilityObj.CreateModel(ability2, CurrentURL(CharacterModels.Ability));
      CRUD<Race> raceObj = new CRUD<Race>();
      raceObj.CreateModel(race, CurrentURL(CharacterModels.Race));
      CRUD<Skill> skillObj = new CRUD<Skill>();
      skillObj.CreateModel(skill1, CurrentURL(CharacterModels.Skill));
      CRUD<Class> classObj = new CRUD<Class>();
      classObj.CreateModel(newClass, CurrentURL(CharacterModels.Class));
      CRUD<Character> characterObj = new CRUD<Character>();
      characterObj.CreateModel(character, CurrentURL(CharacterModels.Character));
    }

    static async void update()
    {
      Skill skill = new Skill()
      {
        ID = 4,
        Name = "Super Speed", //originally Fencing
        Desc = "I go fast" // orginally stabby stabby
      };
      Class newClass = new Class()
      {
        ID = 5,
        ClassName = "Bard", //Orginally Wood Elf
        StatModifier = 3 // originally 5
      };

      Race race = new Race()
      {
        ID = 2,
        RaceType = "Avian", //Originally Giant
        StatModifier = 2 // Originally 100
      };

      Character character = new Character()
      {
        Id = 7,
        UserId = "joiwedfd", //orginally "ewojnfd"
        RaceId = 2, 
        ClassId = 3, 
        Name = "Nebula", // orginally Ameilia
        HP = 3,
        Dex = 4,
        Strength = 8
      };
      CRUD<Ability> abilityObj = new CRUD<Ability>();
      CRUD<Race> raceObj = new CRUD<Race>();
      CRUD<Skill> skillObj = new CRUD<Skill>();
      CRUD<Character> characterObj = new CRUD<Character>();
      CRUD<Class> classObj = new CRUD<Class>();

      Ability updateAbility = new Ability()
      {
        id = 12,
        name = "Charm", //Originally Hi Five
        desc = "Be Done" //Originally Please be done
      };

      Console.WriteLine(abilityObj.UpdateModel(updateAbility, CurrentURL(CharacterModels.Ability) + $"/{updateAbility.id}"));
      Console.WriteLine(raceObj.UpdateModel(race, CurrentURL(CharacterModels.Race) + $"/{race.ID}"));
      Console.WriteLine(skillObj.UpdateModel(skill, CurrentURL(CharacterModels.Skill) + $"/{skill.ID}"));
      Console.WriteLine(characterObj.UpdateModel(character, CurrentURL(CharacterModels.Character) + $"/{character.Id}"));
      Console.WriteLine(classObj.UpdateModel(newClass, CurrentURL(CharacterModels.Class) + $"/{newClass.ID}"));
    }

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



    //static HttpResponseMessage UpdateAbilityAsync(Ability ability)
    //{
    //  try
    //  {
    //    HttpResponseMessage response = null;
    //    string jsonObj = JsonConvert.SerializeObject(ability);
    //    Console.WriteLine(jsonObj);

    //    response = client.PutAsync($"api/Abilities/{ability.id}", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
    //    response.EnsureSuccessStatusCode();
    //    return response;
    //  }
    //  catch (Exception e)
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


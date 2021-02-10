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

      /* ===== Menu Test Calls Start Here ===== */

      //Console.ForegroundColor = ConsoleColor.White;
      //LoginMenu();
      //CreateACharacterMenu();

      /* ===== Menu Test Calls Ends Here ===== */

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


    /* ==================================================================================================== */
    /* ======================================= Menu Method Section ======================================== */
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


  } //end of Program Class
} //end of NameSpace


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
using System.IdentityModel.Tokens.Jwt;

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
      //update();

      //CreateSkill();
      //UpdateSkill();
      //DeleteSkill();

      //CreateClass();
      //DeleteClass();
      //UpdateClass();

      //CreateRace();
      //UpdateRace();
      //DeleteRace();

      //CreateCharacter();
      //UpdateCharacter();

      CreateUserandLogin();

        static void CreateUserandLogin()
        {
            RegisterUser createdUser = new RegisterUser
            {
                Username = "Meep123456789123456789123",
                Email = "Meep123456789123456789123@meep.com",
                Password = "MeepSupreme.1",
                PhoneNumber = "meeeeeeep",
                Roles = new List<string> { "Player" }
            };
            var Result = RegisterNewUser(createdUser);
            Console.WriteLine(Result.ToString());
        }

        static HttpResponseMessage RegisterNewUser(RegisterUser user)
        {
            HttpResponseMessage response = null;
            string jsonObj = JsonConvert.SerializeObject(user);
            Console.WriteLine(jsonObj);
            string password = user.Password;

            try
            {
                response = client.PostAsync("/api/User/Register", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();
                // Handle success
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Code broke");
                // Handle failure
            }
            Task<string> values = response.Content.ReadAsStringAsync();
            UserDTO dtoUser = JsonConvert.DeserializeObject<UserDTO>(values.Result);

            UserDTO userdto = new UserDTO
            {
                Id = dtoUser.Id,
                Username = dtoUser.Username,
                Password = password,
                Token = dtoUser.Token
            };
            var Result = LoginUser(userdto);
            Console.WriteLine(Result.ToString());
            return response;
        }

        static HttpResponseMessage LoginUser(UserDTO user)
        {
            HttpResponseMessage response = null;
            string jsonObj = JsonConvert.SerializeObject(user);
            Console.WriteLine(jsonObj);

            try
            {
                response = client.PostAsync("/api/User/Login", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;

                Console.WriteLine("success, user is logged in-------------------------------------------------");

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

    /* ==================================================================================================== */
    /* =========================================== Test Methods =========================================== */
    /* ==================================================================================================== */

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
        
      RegisterUser user = new RegisterUser
      {
        Username = "Meep12345678912345",
        Email = "Meep12345678912345@meep.com",
        Password = "MeepSupreme.1",
        PhoneNumber = "meeeeeeep",
        Roles = new List<string> { "Player" }
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
        case CharacterModels.User:
            return "/api/User/Register";
        default:
          return "";
      }
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
    Ability,
    User
  }
}// end of namespace


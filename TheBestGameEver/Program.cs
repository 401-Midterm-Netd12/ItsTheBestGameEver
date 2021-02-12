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
using System.Text;

namespace TheBestGameEver
{
    class Program
    {
        static string globalUser = "";
        static string globalUserId = "";
        static HttpClient client = new HttpClient();
        static string URL = "https://customcharacter1.azurewebsites.net";
        static HttpWebRequest WebReq;
        static HttpWebResponse WebResp;
        static CRUD<Character> CRUDCharacter = new CRUD<Character>();
        static CRUD<Race> CRUDRace = new CRUD<Race>();
        static CRUD<Skill> CRUDSkill = new CRUD<Skill>();
        static CRUD<Class> CRUDClass = new CRUD<Class>();
        static CRUD<Ability> CRUDAbility = new CRUD<Ability>();


    static void Main(string[] args)
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            /* === Menu Commands === */

            Console.ForegroundColor = ConsoleColor.White;
            LoginMenu();
  
           //CreateACharacterMenu();

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
        /* =========================================== Menu Methods =========================================== */
        /* ==================================================================================================== */

        static void ConsoleHeader()
        {
            CenterConsoleText(70);
            Console.WriteLine("============================================================================");
            CenterConsoleText(70);
            Console.WriteLine("= ||\\    /||   ||\\    ||                      //////////////               =");
            CenterConsoleText(70);
            Console.WriteLine("= || \\  / ||   || \\   ||                   ////           ////             =");
            CenterConsoleText(70);
            Console.WriteLine("= ||  \\/  ||   ||  \\  ||                ////                ////           =");
            CenterConsoleText(70);
            Console.WriteLine("= ||      ||   ||   \\ ||              ////                   ////          =");
            CenterConsoleText(70);
            Console.WriteLine("= ||      || o ||    \\|| o            ==========|     |==========          =");
            CenterConsoleText(70);
            Console.WriteLine("=                                  =====|  O    |=====|  O    |====        =");
            CenterConsoleText(70);
            Console.WriteLine("=                                   @@@ ========|     |======== @@         =");
            CenterConsoleText(70);
            Console.WriteLine("=                                    @                          @          =");
            CenterConsoleText(70);
            Console.WriteLine("=                                    @@@      |---------|      @@.         =");
            CenterConsoleText(70);
            Console.WriteLine("=                                       @@@   |---------|  *@@#            =");
            CenterConsoleText(70);
            Console.WriteLine("=                                          %@@@@@////#@@@@@.               =");
            CenterConsoleText(70);
            Console.WriteLine("=                                             /////////                    =");
            CenterConsoleText(70);
            Console.WriteLine("============================================================================");

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
            CenterConsoleText(26);
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
                CenterConsoleText(26);
                Console.Write("Welcome to ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("It's the Best Game Ever!\n");
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleText(26);
                Console.WriteLine("\t1. Login");
                CenterConsoleText(26);
                Console.WriteLine("\t2. Register");
                CenterConsoleText(26);
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
            
            string userPwd = "";
            string confirmUserPwd = "";
            string userEmail;
            string userPhone;

            do
            {
                ScreenHeaderDisplay();
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.WriteLine("Please enter your information: \n");
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleText(26);
                Console.Write("User Name: ");
                Console.ForegroundColor = ConsoleColor.White;
                string userName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleText(26);
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.White;
                userPwd = getPasswd();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                CenterConsoleText(26);
                Console.Write("Confirm Password: ");
                Console.ForegroundColor = ConsoleColor.White;
                confirmUserPwd = getPasswd();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                CenterConsoleText(26);
                Console.Write("Email: ");
                Console.ForegroundColor = ConsoleColor.White;
                userEmail = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleText(26);
                Console.Write("Phone Number: ");
                Console.ForegroundColor = ConsoleColor.White;
                userPhone = Console.ReadLine();

                if (userPwd == confirmUserPwd)
                {
                    RegisterUser newUser = new RegisterUser()
                    {
                        Username = userName,
                        Password = userPwd,
                        Email = userEmail,
                        PhoneNumber = userPhone
                    };

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    PushScreenDown(2);
                    CenterConsoleText(26);
                    Console.WriteLine("You Are Now Registered And Logged In!");
                    Console.ForegroundColor = ConsoleColor.White;
                    CreateUserandLogin(newUser);
                    PauseScreen();
                    exit = true;
                    CreateACharacterMenu();
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
                CenterConsoleText(26);
                Console.Write("Login: ");
                Console.ForegroundColor = ConsoleColor.White;
                user = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleText(26);
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.White;
                userPwd = getPasswd();
                exit = true;
            } while (!exit);
        }

        public static void CreateACharacterMenu()
        {
            bool exit;
            string userChoice;

            do
            {
                exit = true;
                ScreenHeaderDisplay();
                CenterConsoleText(26);
                Console.WriteLine($"{globalUser}, what would you like to do?\n");
                CenterConsoleText(26);
                Console.Write("1. Create/Modify a");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Character");
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.Write("2. Create/Modify a");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Race");
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.Write("3. Create/Modify a");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Class");
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.Write("4. Create/Modify a");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(" Ability");
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.Write("5. Create/Modify a");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" Skill");
                Console.ForegroundColor = ConsoleColor.White;
                CenterConsoleText(26);
                Console.WriteLine("6. Exit");
                PushScreenDown(1);
                MakeSelection(6);
                userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        userChoice = "Character";
                        break;
                    case "2":
                        userChoice = "Race";
                        break;
                    case "3":
                        userChoice = "Class";
                        break;
                    case "4":
                        userChoice = "Ability";
                        break;
                    case "5":
                        userChoice = "Skill";
                        break;
                    case "6":
                        userChoice = "Exit";
                        break;
                    default:
                        InvalidSelection();
                        exit = false;
                        break;
                }
            } while (!exit);
            
            if(userChoice != "Exit")
            { 
              ModelSubMenus(userChoice);
            }
      }

      public static void ModelSubMenus(string model)
        {
            bool exit = false;
            string userChoice;

            do
            {
                ScreenHeaderDisplay();
                CenterConsoleText(26);
                Console.Write($"What would you like to do in ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0}, {1}?\n", model, globalUser);
                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Retrieve All");

                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Retrieve a Specific One");



                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("3. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Create a New One");



                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("4. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Modify a Specific One");


                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("5. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Delete a Specific One");



                CenterConsoleText(26);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("6. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Exit");
                PushScreenDown(1);
                MakeSelection(6);
                userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        Console.Clear();
                        PushScreenDown(2);
                        ConsoleHeader();
                        GetSwitch(model);
                        PauseScreen();
                        break;
                    case "2":
                        Console.Clear();
                        PushScreenDown(2);
                        ConsoleHeader();
                        Console.WriteLine($"What is the Id of the {model} would you like to look at?");

                        Console.WriteLine("Do you have the Id you need? Y/N");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            Console.WriteLine("What Id would you like to retrieve?");
                            userChoice = Console.ReadLine();
                            Console.Clear();
                            PushScreenDown(2);
                            ConsoleHeader();
                            GetSwitch(model, Convert.ToInt32(userChoice));

                        }
                        else
                        {
                            Console.WriteLine($"You can get the Id by Retrieving all {model}s in the previous menu.");
                        }
                        PauseScreen();
                        break;
                    case "3":
                        Console.Clear();
                        PushScreenDown(2);
                        ConsoleHeader();
                        CreateSwitch(model);
                        break;
                    case "4":
                        Console.Clear();
                        PushScreenDown(2);
                        ConsoleHeader();
                        UpdateSwitch(model);
                        break;
                    case "5":
                        Console.Clear();
                        PushScreenDown(2);
                        ConsoleHeader();
                        DeleteSwitch(model);
                        break;
                    case "6":
                        exit = true;
                        CreateACharacterMenu();
                        break;
                    default:
                        InvalidSelection();
                        break;
                }
            } while (!exit);
        }

        /*=========================================================================================================================*/
        /*==================================================  Helper Switch Methods  ==============================================*/
        /*=========================================================================================================================*/
        public static void GetSwitch(string model, int id = 0)
        {
            switch (model)
            {
                case "Character":
                    CharacterDisplay(id);
                    break;
                case "Race":
                    RaceDisplay(id);
                    break;
                case "Class":
                    ClassDisplay(id);
                    break;
                case "Ability":
                    AbilityDisplay(id);
                    break;
                case "Skill":
                    SkillDisplay(id);
                    break;
                default:
                    InvalidSelection();
                    break;
            }
        }
        public static void DeleteSwitch(string model, int id = 0)
        {
            switch (model)
            {
                case "Character":
                    DeleteCharacter(id);
                    break;
                case "Race":
                    DeleteRace(id);
                    break;
                case "Class":
                    DeleteClass(id);
                    break;
                case "Ability":
                    DeleteAbility(id);
                    break;
                case "Skill":
                    DeleteSkill(id);
                    break;
                default:
                    InvalidSelection();
                    break;
            }
        }
        public static void CreateSwitch(string model)
        {
            switch (model)
            {
                case "Character":
                    CreateCharacter();
                    break;
                case "Race":
                    CreateRace();
                    break;
                case "Class":
                    CreateClass();
                    break;
                case "Ability":
                    CreateAbility();
                    break;
                case "Skill":
                    CreateSkill();
                    break;
                default:
                    InvalidSelection();
                    break;
            }
        }
        public static void UpdateSwitch(string model)
        {
            switch (model)
            {
                case "Character":
                    UpdateCharacter();
                    break;
                case "Race":
                    UpdateRace();
                    break;
                case "Class":
                    UpdateClass();
                    break;
                case "Ability":
                    UpdateAbility();
                    break;
                case "Skill":
                    UpdateSkill();
                    break;
                default:
                    InvalidSelection();
                    break;
            }
        }
        /*=========================================================================================================================*/
        /*=====================================================   Login Stuff  ====================================================*/
        /*=========================================================================================================================*/

        static void CreateUserandLogin(RegisterUser createdUser)
        {
            createdUser.Roles = new List<string> { "Player" };
            var Result = RegisterNewUser(createdUser);

        }
        static HttpResponseMessage RegisterNewUser(RegisterUser user)
        {
            HttpResponseMessage response = null;
            string jsonObj = JsonConvert.SerializeObject(user);
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
            globalUser = userdto.Username;
            globalUserId = userdto.Id;
            var Result = LoginUser(userdto);

            return response;
        }
        static HttpResponseMessage LoginUser(UserDTO user)
        {
            HttpResponseMessage response = null;
            string jsonObj = JsonConvert.SerializeObject(user);

            try
            {
                response = client.PostAsync("/api/User/Login", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
                Console.WriteLine("Login Successful!");
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

        /*=========================================================================================================================*/
        /*=====================================================   Get Methods  ====================================================*/
        /*=========================================================================================================================*/

        static async void AbilityDisplay(int id = 0)
        {
            List<Ability> abilityList = new List<Ability>();
            abilityList = await CRUDAbility.GetModels(CurrentURL(CharacterModels.Ability), id);
            foreach (Ability item in abilityList)
            {
                Console.Write("\tID: ");
                Console.WriteLine(item.Id);
                Console.Write("\tName: ");
                Console.WriteLine(item.Name);
                Console.Write("\tDesc: ");
                Console.WriteLine(item.Desc);
                PushScreenDown(2);
            }
        }
        static async void SkillDisplay(int id = 0) // pulled all ids so far
        {
            List<Skill> skillList = new List<Skill>();
            skillList = await CRUDSkill.GetModels(CurrentURL(CharacterModels.Skill), id);
            foreach (Skill item in skillList)
            {
                Console.Write("\tID: ");
                Console.WriteLine(item.ID);
                Console.Write("\tName: ");
                Console.WriteLine(item.Name);
                Console.Write("\tDesc: ");
                Console.WriteLine(item.Desc);
                PushScreenDown(2);
      }
    }
        static async void RaceDisplay(int id = 0)
        {
            List<Race> raceList = new List<Race>();
            raceList = await CRUDRace.GetModels(CurrentURL(CharacterModels.Race), id);
            foreach (Race item in raceList)
            {
                Console.Write("\tID: ");
                Console.WriteLine(item.ID);
                Console.Write("\tRace Type: ");
                Console.WriteLine(item.RaceType);
                Console.Write("\tStat Modifier: ");
                Console.WriteLine(item.StatModifer);
                PushScreenDown(2);
      }
    }
        static async void ClassDisplay(int id = 0)
        {
            List<Class> classList = new List<Class>();
            classList = await CRUDClass.GetModels(CurrentURL(CharacterModels.Class), id);
            foreach (Class item in classList)
            {
                Console.Write("\tID: ");
                Console.WriteLine(item.ID);
                Console.Write("\tName: ");
                Console.WriteLine(item.ClassName);
                Console.Write("\tStat Modifier: ");
                Console.WriteLine(item.StatModifier);
                PushScreenDown(2);
      }
    }

        static async void CharacterDisplay(int id = 0)
        {
            List<Character> characterList = new List<Character>();
            characterList = await CRUDCharacter.GetModels(CurrentURL(CharacterModels.Character), id);
            foreach (Character item in characterList)
            {
                Console.WriteLine();
                Console.Write("\tID: ");
                Console.WriteLine(item.Id);
                Console.Write("\tUserName: ");
                Console.WriteLine(item.Name);
                Console.Write("\tRaceId: ");
                Console.WriteLine(item.RaceId);
                Console.Write("\tClassId: ");
                Console.WriteLine(item.ClassId);
                Console.Write("\tUserId: ");
                Console.WriteLine(item.UserId);
                Console.Write("\tHP: ");
                Console.WriteLine(item.HP);
                Console.Write("\tDex: ");
                Console.WriteLine(item.Dex);
                Console.Write("\tStrength: ");
                Console.WriteLine(item.Strength);
                Console.WriteLine();
                
            }
        }
        /*=========================================================================================================================*/
        /*=====================================================   Delete Methods  ====================================================*/
        /*=========================================================================================================================*/
        static async void DeleteCharacter(int id = 0)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Characters");
                Console.WriteLine("\t2. Delete a Character");
                Console.WriteLine("\t3. Exit\n");
                Console.Write("\t");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("");
                        CharacterDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("\tPlease enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await CRUDCharacter.DeleteModels(answerToInt, CurrentURL(CharacterModels.Character));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void DeleteClass(int id = 0)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Classes");
                Console.WriteLine("\t2. Delete a Class");
                Console.WriteLine("\t3. Exit\n");
                Console.Write("\t");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("");
                        ClassDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("\tPlease enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await CRUDClass.DeleteModels(answerToInt, CurrentURL(CharacterModels.Class));
                        break;
                    case "3":
                        exit = true;
                        break;
                }

            }
        }
        static async void DeleteSkill(int id = 0)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Skills");
                Console.WriteLine("\t2. Delete a Skill");
                Console.WriteLine("\t3. Exit\n");
                Console.Write("\t");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("");
                        SkillDisplay(id);
                    
                        break;
                    case "2":
                        Console.WriteLine("\tPlease enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await CRUDSkill.DeleteModels(answerToInt, CurrentURL(CharacterModels.Skill));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void DeleteAbility(int id = 0)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Abilities");
                Console.WriteLine("\t2. Delete an Ability");
                Console.WriteLine("\t3. Exit\n");
                Console.Write("\t");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("");
                        AbilityDisplay(id);
                    
                        break;
                    case "2":
                        Console.WriteLine("\tPlease enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await CRUDAbility.DeleteModels(answerToInt, CurrentURL(CharacterModels.Ability));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        static async void DeleteRace(int id = 0)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Races");
                Console.WriteLine("\t2. Delete a Race");
                Console.WriteLine("\t3. Exit\n");
                Console.Write("\t");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("");
                        RaceDisplay(id);
                    
                        break;
                    case "2":
                        Console.WriteLine("\tPlease enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await CRUDRace.DeleteModels(answerToInt, CurrentURL(CharacterModels.Race));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        /*=========================================================================================================================*/
        /*==================================================   Create Methods  ====================================================*/
        /*=========================================================================================================================*/

        static async void CreateAbility(int id = 0)
        {
            List<Ability> abilitiesList = new List<Ability>();
            Ability ability = new Ability();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Abilities");
                Console.WriteLine("\t2. Create an Ability");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        abilitiesList = await CRUDAbility.GetModels(CurrentURL(CharacterModels.Ability));
                        foreach (Ability item in abilitiesList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tAbility Name: " + item.Name);
                            Console.WriteLine("\tAbility Description: " + item.Desc);
                            Console.WriteLine("");

                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease create a name for this ability: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        ability.Name = response;
                        Console.WriteLine("\tPlease create a description for this ability: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        ability.Desc = response1;
                        CRUDAbility.CreateModel(ability, CurrentURL(CharacterModels.Ability));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateSkill()
        {
            List<Skill> skillList = new List<Skill>();
            Skill skill = new Skill();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Skills");
                Console.WriteLine("\t2. Create a Skill");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        skillList = await CRUDSkill.GetModels(CurrentURL(CharacterModels.Skill));
                        foreach (Skill item in skillList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tSkill Name: " + item.Name);
                            Console.WriteLine("\tSkill Description: " + item.Desc);
                            Console.WriteLine("");

                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease create a name for this skill: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        skill.Name = response;
                        Console.WriteLine("\tPlease create a description for this skill: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        skill.Desc = response1;
                        CRUDSkill.CreateModel(skill, CurrentURL(CharacterModels.Skill));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateRace()
        {
            List<Race> raceList = new List<Race>();
            Race race = new Race();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Races");
                Console.WriteLine("\t2. Create a Race");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        raceList = await CRUDRace.GetModels(CurrentURL(CharacterModels.Race));
                        foreach (Race item in raceList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tRace: " + item.RaceType);
                            Console.WriteLine("\tStatModifier: " + item.StatModifer);
                            Console.WriteLine("");

                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease create a race: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        race.RaceType = response1;
                        Console.WriteLine("\tPlease create a stat modifier(number 1-10): ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        int response2 = Convert.ToInt32(response);
                        race.StatModifer = response2;
                        CRUDRace.CreateModel(race, CurrentURL(CharacterModels.Race));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateClass()
        {
            List<Class> ClassList = new List<Class>();
            Class testClass = new Class();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Class");
                Console.WriteLine("\t2. Create a Class");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ClassList = await CRUDClass.GetModels(CurrentURL(CharacterModels.Class));
                        foreach (Class item in ClassList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tClass Name: " + item.ClassName);
                            Console.WriteLine("\tStatModifier: " + item.StatModifier);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease create a Class: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        testClass.ClassName = response1;
                        Console.WriteLine("\tPlease create a stat modifier(number 1-10): ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        int response2 = Convert.ToInt32(response);
                        testClass.StatModifier = response2;
                        CRUDClass.CreateModel(testClass, CurrentURL(CharacterModels.Class));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateCharacter()
        {
            List<Character> characterList = new List<Character>();
            List<Race> raceList = new List<Race>();
            List<Class> classList = new List<Class>();
            Character testCharacter = new Character();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Character");
                Console.WriteLine("\t2. View Race IDs");
                Console.WriteLine("\t3. View Class IDs");
                Console.WriteLine("\t4. Create a Character (we recommend viewing races and classes first!)");
                Console.WriteLine("\t5. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        characterList = await CRUDCharacter.GetModels(CurrentURL(CharacterModels.Character));
                        foreach (Character item in characterList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tCharacter Name: " + item.Name);
                            Console.WriteLine("\tHP: " + item.HP);
                            Console.WriteLine("\tDexterity: " + item.Dex);
                            Console.WriteLine("\tStrength: " + item.Strength);
                            Console.WriteLine("");

                        }
                        break;
                    case "2":
                        raceList = await CRUDRace.GetModels(CurrentURL(CharacterModels.Race));
                        foreach (Race item in raceList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tRace: " + item.RaceType);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\tRaceID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.ID);
                            Console.WriteLine("");

                        }
                        break;
                    case "3":
                        classList = await CRUDClass.GetModels(CurrentURL(CharacterModels.Class));
                        foreach (Class item in classList)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\tClass: " + item.ClassName);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\tClassID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.ID);
                            Console.WriteLine("");

                        }
                        break;
                    case "4":
                        Console.WriteLine("\tPlease create a new character name: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        testCharacter.Name = response;
                        Console.WriteLine("\tPlease select a race ID (hint: you can find this in option 3): ");
            Console.Write("\t");
            string response2 = Console.ReadLine();
                        int responseToInt2 = Convert.ToInt32(response2);
                        testCharacter.RaceId = responseToInt2;
                        Console.WriteLine("\tPlease select a class ID (hint: you can find this in option 2): ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        int responseToInt = Convert.ToInt32(response1);
                        testCharacter.ClassId = responseToInt;
                        Console.WriteLine("\tPlease select an HP (enter a number between 1-10): ");
            Console.Write("\t");
            string response3 = Console.ReadLine();
                        int responseToInt3 = Convert.ToInt32(response3);
                        testCharacter.HP = responseToInt3;
                        Console.WriteLine("\tPlease select a Dexterity (enter a number between 1-10): ");
            Console.Write("\t");
            string response4 = Console.ReadLine();
                        int responseToInt4 = Convert.ToInt32(response4);
                        testCharacter.Dex = responseToInt4;
                        Console.WriteLine("\tPlease select a Strength (enter a number between 1-20): ");
            Console.Write("\t");
            string response5 = Console.ReadLine();
                        int responseToInt5 = Convert.ToInt32(response5);
                        testCharacter.Strength = responseToInt5;
                        testCharacter.UserId = globalUserId;
                        testCharacter.CharClass = new Class { };
                        testCharacter.CharRace = new Race { };
                        CRUDCharacter.CreateModel(testCharacter, CurrentURL(CharacterModels.Character));
                        break;
                    case "5":
                        exit = true;
                        break;
                }
            }
        }

        /*=========================================================================================================================*/
        /*==================================================   Update Methods  ====================================================*/
        /*=========================================================================================================================*/
        static async void UpdateAbility(int id = 0)
        {
            List<Ability> abilitiesList = new List<Ability>();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Abilities");
                Console.WriteLine("\t2. Update an Ability");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        abilitiesList = await CRUDAbility.GetModels(CurrentURL(CharacterModels.Ability));
                        foreach (Ability item in abilitiesList)
                        {
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\tAbility ID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.Id);
                            Console.WriteLine("\tAbility Name: " + item.Name);
                            Console.WriteLine("\tAbility Description: " + item.Desc);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please select the id of the ability you would like to update: ");
            Console.Write("\t");
            string responseAbility = Console.ReadLine();
                        int responseInt = Convert.ToInt32(responseAbility);
                        abilitiesList = await CRUDAbility.GetModels(CurrentURL(CharacterModels.Ability), responseInt);
                        Ability ability = new Ability()
                        {
                            Id = abilitiesList.First().Id,
                            Name = abilitiesList.First().Name,
                            Desc = abilitiesList.First().Desc
                        };
                        Console.WriteLine("\tPlease update the name for this ability: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        ability.Name = response;
                        Console.WriteLine("\tPlease update the description for this ability: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        ability.Desc = response1;
                        CRUDAbility.UpdateModel(ability, CurrentURL(CharacterModels.Ability) + $"/{ability.Id}");
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        static async void UpdateSkill(int id = 0)
        {
            List<Skill> skillList = new List<Skill>();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Skills");
                Console.WriteLine("\t2. Update an Skill");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        skillList = await CRUDSkill.GetModels(CurrentURL(CharacterModels.Skill));
                        foreach (Skill item in skillList)
                        {
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Skill ID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.ID);
                            Console.WriteLine("\tSkill Name: " + item.Name);
                            Console.WriteLine("\tSkill Description: " + item.Desc);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease select the id of the Skill you would like to update: ");
            Console.Write("\t");
            string responseSkill = Console.ReadLine();
                        int responseInt = Convert.ToInt32(responseSkill);
                        skillList = await CRUDSkill.GetModels(CurrentURL(CharacterModels.Skill), responseInt);
                        Skill Skill = new Skill()
                        {
                            ID = skillList.First().ID,
                            Name = skillList.First().Name,
                            Desc = skillList.First().Desc
                        };
                        Console.WriteLine("\tPlease update the name for this Skill: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        Skill.Name = response;
                        Console.WriteLine("\tPlease update the description for this Skill: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        Skill.Desc = response1;
                        CRUDSkill.UpdateModel(Skill, CurrentURL(CharacterModels.Skill) + $"/{Skill.ID}");
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void UpdateRace(int id = 0)
        {
            List<Race> RaceList = new List<Race>();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Races");
                Console.WriteLine("\t2. Update an Race");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        RaceList = await CRUDRace.GetModels(CurrentURL(CharacterModels.Race));
                        foreach (Race item in RaceList)
                        {
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\tRace ID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.ID);
                            Console.WriteLine("\tRace Type: " + item.RaceType);
                            Console.WriteLine("\tStatModifier: " + item.StatModifer);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please select the id of the Race you would like to update: ");
            Console.Write("\t");
            string responseRace = Console.ReadLine();
                        int responseInt = Convert.ToInt32(responseRace);
                        RaceList = await CRUDRace.GetModels(CurrentURL(CharacterModels.Race), responseInt);
                        Race Race = new Race()
                        {
                            ID = RaceList.First().ID,
                            RaceType = RaceList.First().RaceType,
                            StatModifer = RaceList.First().StatModifer
                        };
                        Console.WriteLine("\tPlease update the race type: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        Race.RaceType = response;
                        Console.WriteLine("\tPlease update the stat modifier for this Race: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        int statResponse = Convert.ToInt32(response1);
                        Race.StatModifer = statResponse;
                        CRUDRace.UpdateModel(Race, CurrentURL(CharacterModels.Race) + $"/{Race.ID}");
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        static async void UpdateClass(int id = 0)
        {
            List<Class> ClassList = new List<Class>();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Classes");
                Console.WriteLine("\t2. Update an Class");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ClassList = await CRUDClass.GetModels(CurrentURL(CharacterModels.Class));
                        foreach (Class item in ClassList)
                        {
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\tClass ID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.ID);
                            Console.WriteLine("\tClass Name: " + item.ClassName);
                            Console.WriteLine("\tStatModifier: " + item.StatModifier);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease select the id of the Class you would like to update: ");
            Console.Write("\t");
            string responseClass = Console.ReadLine();
                        int responseInt = Convert.ToInt32(responseClass);
                        ClassList = await CRUDClass.GetModels(CurrentURL(CharacterModels.Class), responseInt);
                        Class Class = new Class()
                        {
                            ID = ClassList.First().ID,
                            ClassName = ClassList.First().ClassName,
                            StatModifier = ClassList.First().StatModifier
                        };
                        Console.WriteLine("\tPlease update the class name: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        Class.ClassName = response;
                        Console.WriteLine("\tPlease update the stat modifier for this class: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        int statResponse = Convert.ToInt32(response1);
                        Class.StatModifier = statResponse;
                        CRUDClass.UpdateModel(Class, CurrentURL(CharacterModels.Class) + $"/{Class.ID}");
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        static async void UpdateCharacter(int id = 0)
        {
            List<Character> CharacterList = new List<Character>();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\t1. View Characters");
                Console.WriteLine("\t2. Update an Character");
                Console.WriteLine("\t3. Exit");
        Console.Write("\t");
        string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        CharacterList = await CRUDCharacter.GetModels(CurrentURL(CharacterModels.Character));
                        foreach (Character item in CharacterList)
                        {
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\tCharacter ID: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(item.Id);
                            Console.WriteLine("\tCharacter Name: " + item.Name);
                            Console.WriteLine("\tHP: " + item.HP);
                            Console.WriteLine("\tDex: " + item.Dex);
                            Console.WriteLine("\tStrength: " + item.Strength);
                            Console.WriteLine("");
                        }
                        break;
                    case "2":
                        Console.WriteLine("\tPlease select the id of the Character you would like to update: ");
            Console.Write("\t");
            string responseCharacter = Console.ReadLine();
                        int responseInt = Convert.ToInt32(responseCharacter);
                        CharacterList = await CRUDCharacter.GetModels(CurrentURL(CharacterModels.Character), responseInt);
                        Character Character = new Character()
                        {
                            Id = CharacterList.First().Id,
                            Name = CharacterList.First().Name,
                            HP = CharacterList.First().HP,
                            Dex = CharacterList.First().Dex,
                            Strength = CharacterList.First().Strength,
                            RaceId = CharacterList.First().RaceId,
                            UserId = CharacterList.First().UserId,
                            ClassId = CharacterList.First().ClassId,
                            CharClass = CharacterList.First().CharClass,
                            CharRace = CharacterList.First().CharRace
                        };
                        Console.WriteLine("\tPlease update the Character name: ");
            Console.Write("\t");
            string response = Console.ReadLine();
                        Character.Name = response;
                        Console.WriteLine("\tPlease update the HP for this Character: ");
            Console.Write("\t");
            string response1 = Console.ReadLine();
                        int statResponse = Convert.ToInt32(response1);
                        Character.HP = statResponse;
                        Console.WriteLine("\tPlease update the Dex for this Character: ");
            Console.Write("\t");
            string response2 = Console.ReadLine();
                        int statResponse2 = Convert.ToInt32(response2);
                        Character.Dex = statResponse2;
                        Console.WriteLine("\tPlease update the Strength for this Character: ");
            Console.Write("\t");
            string response3 = Console.ReadLine();
                        int statResponse3 = Convert.ToInt32(response3);
                        Character.Strength = statResponse3;
                        CRUDCharacter.UpdateModel(Character, CurrentURL(CharacterModels.Character) + $"/{Character.Id}");
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
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


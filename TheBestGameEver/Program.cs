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
        static string globalUser = "";
        static string globalUserId = "";
        static HttpClient client = new HttpClient();
        static string URL = "https://customcharacter1.azurewebsites.net";
        static HttpWebRequest WebReq;
        static HttpWebResponse WebResp;
        static void Main(string[] args)
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            /* === Menu Commands === */

            Console.ForegroundColor = ConsoleColor.White;
            //LoginMenu();
  
            CreateACharacterMenu();

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
            bool exit = false;
            string userChoice;

            do
            {
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
                        ModelSubMenus("Character");
                        break;
                    case "2":
                        ModelSubMenus("Race");
                        break;
                    case "3":
                        ModelSubMenus("Class");
                        break;
                    case "4":
                        ModelSubMenus("Ability");
                        break;
                    case "5":
                        ModelSubMenus("Skill");
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
                        break;
                    case "5":
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
            CRUD<Ability> abilityObj = new CRUD<Ability>();
            abilityList = await abilityObj.GetModels(CurrentURL(CharacterModels.Ability), id);
            foreach (Ability item in abilityList)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("ID: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Id);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Name: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Name);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Desc: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Desc);

            }
        }
        static async void SkillDisplay(int id = 0) // pulled all ids so far
        {
            List<Skill> skillList = new List<Skill>();
            CRUD<Skill> skillObj = new CRUD<Skill>();
            skillList = await skillObj.GetModels(CurrentURL(CharacterModels.Skill), id);
            foreach (Skill item in skillList)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("ID: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.ID);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Name: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Name);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Desc: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Desc);
            }
        }
        static async void RaceDisplay(int id = 0)
        {
            List<Race> raceList = new List<Race>();
            CRUD<Race> raceObj = new CRUD<Race>();
            raceList = await raceObj.GetModels(CurrentURL(CharacterModels.Race), id);
            foreach (Race item in raceList)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("ID: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.ID);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Race Type: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.RaceType);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Stat Modifier: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.StatModifer);
            }
        }
        static async void ClassDisplay(int id = 0)
        {
            List<Class> classList = new List<Class>();
            CRUD<Class> classObj = new CRUD<Class>();
            classList = await classObj.GetModels(CurrentURL(CharacterModels.Class), id);
            foreach (Class item in classList)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("ID: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.ID);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Name: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.ClassName);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Stat Modifier: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.StatModifier);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static async void CharacterDisplay(int id = 0)
        {
            List<Character> characterList = new List<Character>();
            CRUD<Character> characterObj = new CRUD<Character>();
            characterList = await characterObj.GetModels(CurrentURL(CharacterModels.Character), id);
            foreach (Character item in characterList)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine();
                Console.Write("ID: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Id);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("UserName: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Name);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("RaceId: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.RaceId);
                Console.Write("ClassId: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.ClassId);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("UserId: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.UserId);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("HP: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.HP);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Dex: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.Dex);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Strength: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
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
            CRUD<Character> characterObj = new CRUD<Character>();
            while (!exit)
            {
                Console.WriteLine("1. View Characters");
                Console.WriteLine("2. Delete a Character");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        CharacterDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("Please enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await characterObj.DeleteModels(answerToInt, CurrentURL(CharacterModels.Character));
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
            CRUD<Class> classObj = new CRUD<Class>();
            while (!exit)
            {
                Console.WriteLine("1. View Classes");
                Console.WriteLine("2. Delete a Class");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ClassDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("Please enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await classObj.DeleteModels(answerToInt, CurrentURL(CharacterModels.Class));
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
            CRUD<Skill> skillObj = new CRUD<Skill>();
            while (!exit)
            {
                Console.WriteLine("1. View Skills");
                Console.WriteLine("2. Delete a Skill");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        SkillDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("Please enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await skillObj.DeleteModels(answerToInt, CurrentURL(CharacterModels.Skill));
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
            CRUD<Ability> abilityObj = new CRUD<Ability>();
            while (!exit)
            {
                Console.WriteLine("1. View Abilities");
                Console.WriteLine("2. Delete an Ability");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AbilityDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("Please enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await abilityObj.DeleteModels(answerToInt, CurrentURL(CharacterModels.Ability));
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
            CRUD<Race> raceObj = new CRUD<Race>();
            while (!exit)
            {
                Console.WriteLine("1. View Races");
                Console.WriteLine("2. Delete a Race");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        RaceDisplay(id);
                        break;
                    case "2":
                        Console.WriteLine("Please enter an Id to delete: ");
                        string response = Console.ReadLine();
                        int answerToInt = Convert.ToInt32(response);
                        await raceObj.DeleteModels(answerToInt, CurrentURL(CharacterModels.Race));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        /*=========================================================================================================================*/
        /*=====================================================   Create Methods  ====================================================*/
        /*=========================================================================================================================*/

        static async void CreateAbility(int id = 0)
        {
            CRUD<Ability> abilityObj = new CRUD<Ability>();
            List<Ability> abilitiesList = new List<Ability>();
            Ability ability = new Ability();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. View Abilities");
                Console.WriteLine("2. Create an Ability");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        abilitiesList = await abilityObj.GetModels(CurrentURL(CharacterModels.Ability));
                        foreach (Ability item in abilitiesList)
                        {
                            Console.WriteLine("Ability Name: " + item.Name);
                            Console.WriteLine("Ability Description: " + item.Desc);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please create a name for this ability: ");
                        string response = Console.ReadLine();
                        ability.Name = response;
                        Console.WriteLine("Please create a description for this ability: ");
                        string response1 = Console.ReadLine();
                        ability.Desc = response1;
                        abilityObj.CreateModel(ability, CurrentURL(CharacterModels.Ability));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateSkill()
        {
            CRUD<Skill> skillObj = new CRUD<Skill>();
            List<Skill> skillList = new List<Skill>();
            Skill skill = new Skill();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. View Skills");
                Console.WriteLine("2. Create a Skill");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        skillList = await skillObj.GetModels(CurrentURL(CharacterModels.Skill));
                        foreach (Skill item in skillList)
                        {
                            Console.WriteLine("Skill Name: " + item.Name);
                            Console.WriteLine("Skill Description: " + item.Desc);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please create a name for this skill: ");
                        string response = Console.ReadLine();
                        skill.Name = response;
                        Console.WriteLine("Please create a description for this skill: ");
                        string response1 = Console.ReadLine();
                        skill.Desc = response1;
                        skillObj.CreateModel(skill, CurrentURL(CharacterModels.Skill));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateRace()
        {
            CRUD<Race> raceObj = new CRUD<Race>();
            List<Race> raceList = new List<Race>();
            Race race = new Race();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. View Races");
                Console.WriteLine("2. Create a Race");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        raceList = await raceObj.GetModels(CurrentURL(CharacterModels.Race));
                        foreach (Race item in raceList)
                        {
                            Console.WriteLine("Race: " + item.RaceType);
                            Console.WriteLine("StatModifier: " + item.StatModifer); // did not see stat modifier
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please create a stat modifier(number 1-10): ");
                        string response = Console.ReadLine();
                        int response2 = Convert.ToInt32(response);
                        race.StatModifer = response2;
                        Console.WriteLine("Please create a race: ");
                        string response1 = Console.ReadLine();
                        race.RaceType = response1;
                        raceObj.CreateModel(race, CurrentURL(CharacterModels.Race));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateClass()
        {
            CRUD<Class> classObj = new CRUD<Class>();
            List<Class> ClassList = new List<Class>();
            Class testClass = new Class();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. View Class");
                Console.WriteLine("2. Create a Class");
                Console.WriteLine("3. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ClassList = await classObj.GetModels(CurrentURL(CharacterModels.Class));
                        foreach (Class item in ClassList)
                        {
                            Console.WriteLine("Class Name: " + item.ClassName);
                            Console.WriteLine("StatModifier: " + item.StatModifier);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please create a stat modifier(number 1-10): ");
                        string response = Console.ReadLine();
                        int response2 = Convert.ToInt32(response);
                        testClass.StatModifier = response2;
                        Console.WriteLine("Please create a Class: ");
                        string response1 = Console.ReadLine();
                        testClass.ClassName = response1;
                        classObj.CreateModel(testClass, CurrentURL(CharacterModels.Class));
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }
        static async void CreateCharacter()
        {
            CRUD<Character> characterObj = new CRUD<Character>();
            List<Character> characterList = new List<Character>();
            CRUD<Race> raceObj = new CRUD<Race>();
            List<Race> raceList = new List<Race>();
            CRUD<Class> classObj = new CRUD<Class>();
            List<Class> classList = new List<Class>();
            Character testCharacter = new Character();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("1. View Character");
                Console.WriteLine("2. View Race IDs");
                Console.WriteLine("3. View Class IDs");
                Console.WriteLine("4. Create a Character (we recommend viewing races and classes first!)");
                Console.WriteLine("5. Exit");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        characterList = await characterObj.GetModels(CurrentURL(CharacterModels.Character));
                        foreach (Character item in characterList)
                        {
                            Console.WriteLine("Character Name: " + item.Name);
                            Console.WriteLine("HP: " + item.HP);
                            Console.WriteLine("Dexterity: " + item.Dex);
                            Console.WriteLine("Strength: " + item.Strength);
                            Console.WriteLine($"Created By: {globalUser}");
                        }
                        break;
                    case "2":
                        raceList = await raceObj.GetModels(CurrentURL(CharacterModels.Race));
                        foreach (Race item in raceList)
                        {
                            Console.WriteLine("Race: " + item.RaceType);
                            Console.WriteLine("RaceID: " + item.ID);
                        }
                        break;
                    case "3":
                        classList = await classObj.GetModels(CurrentURL(CharacterModels.Class));
                        foreach (Class item in classList)
                        {
                            Console.WriteLine("Class: " + item.ClassName);
                            Console.WriteLine("ClassID: " + item.ID);
                        }
                        break;
                    case "4":
                        Console.WriteLine("Please create a new character name: ");
                        string response = Console.ReadLine();
                        testCharacter.Name = response;
                        Console.WriteLine("Please select a class ID (hint: you can find this in option 2): ");
                        string response1 = Console.ReadLine();
                        int responseToInt = Convert.ToInt32(response1);
                        testCharacter.ClassId = responseToInt;
                        Console.WriteLine("Please select a race ID (hint: you can find this in option 3): ");
                        string response2 = Console.ReadLine();
                        int responseToInt2 = Convert.ToInt32(response2);
                        testCharacter.RaceId = responseToInt2;
                        Console.WriteLine("Please select an HP (enter a number between 1-10): ");
                        string response3 = Console.ReadLine();
                        int responseToInt3 = Convert.ToInt32(response3);
                        testCharacter.HP = responseToInt3;
                        Console.WriteLine("Please select a Dexterity (enter a number between 1-10): ");
                        string response4 = Console.ReadLine();
                        int responseToInt4 = Convert.ToInt32(response4);
                        testCharacter.Dex = responseToInt4;
                        Console.WriteLine("Please select a Strength (enter a number between 1-20): ");
                        string response5 = Console.ReadLine();
                        int responseToInt5 = Convert.ToInt32(response5);
                        testCharacter.Strength = responseToInt5;
                        testCharacter.UserId = globalUserId;
                        testCharacter.CharClass = new Class { };
                        testCharacter.CharRace = new Race { };
                        characterObj.CreateModel(testCharacter, CurrentURL(CharacterModels.Character));
                        break;
                    case "5":
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


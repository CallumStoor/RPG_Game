using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{

    enum Level
    {
        Title = 0,
        Intro, 
        _01,
        _02
    }

    public static void Main(string[] args)
    {
        // starting stats of the player at the begining of the game
        Player user = new Player("", 10, 1, 1, 1, 1, 0);

        // rooms in the game
        Room street = new Room("Street", "You are standing on a busy street, it smells and everyone is moving past you with somewhere to go.", "The people look at you with disgust");
        Room shop = new Room("Shop", "You are in a small shop, there is a shopkeep behind the counter eyeing you suspiciously", "You enter the shop and look around, you still have no money... \nYou walk over to the man behind the till and wait in the long line \nWhen you finally reach the man he greets you with a disapointed face, probably becuase you look like a street begger\n\n 'Get out of here I won't give you anything' \n 'No wait' you exclaim 'I need a job!' \n 'Come back another time you aren't the only one' \n\nYou leave the store and look at the setting sun.");
        
        // current room the player is in
        Room currentRoom = street;

        // linking rooms together
        street.NorthDoor = shop;

        shop.SouthDoor = street;

        // game variables
        bool isAlive = true;
        bool newGame = false;
        string Answer = "";
        int DaylyMoney = 0;
        Level currentLevel = Level.Title;

        // game starts here

        Console.ResetColor();

        WelcomeScreen();

        if (newGame == true) // if starting a new game run through the intro and levels
        {

            IntroScene();

            Level01();

            Level02();       
        }

        AliveCheck(); // just in case

        Console.ReadLine();




        void WelcomeScreen() // Infomation about how to play and Level selection
        {
            user.PlayerName = PlayerInput("What is your name: ");

            ScrollText("Type 'help' to see game comands\n");
            ScrollText(CentrePad("Welcome to Bog Dewller", 2));

            Answer = choice("What level do you want to start at? \n1. Start New Game \n2. Level 1\n3. Level 2", 3);
            switch (Answer)
            {
                case "1":
                    ScrollText("Starting New Game");
                    newGame = true;
                    break;
                case "2":
                    ScrollText("Starting at Level 1");
                    Level01();
                    break;
                case "3":
                    ScrollText("Starting at Level 2");
                    Level02();
                    break;
            }


        }

        void IntroScene()
        {
            currentLevel = Level.Intro;
            Console.Clear();
            ScrollText($"Welcome! {user.PlayerName}");

            //show image
            AsciiImage("Village");

            // shows strings with a wait after 
            ScrollText("This is the great town of ");
            ScrollText("You don't know yet..");

            Continue();

            Console.Clear();
            Console.WriteLine("Intro to game here");

            Continue();

        }

        void Level01()
        {
            currentLevel = Level._01;

            Console.Clear();

            ScrollText("You flee into the city running as fast as your legs will take you");
            ScrollText("No looking back now.");
            ScrollText("What will you do?");

            Answer = choice("1. Continue to run away\n2. Hide in the crowds of people (Sneak)", 2);

            switch (Answer)
            {
                case "1":
                    ScrollText("You run down an alley way disapearing into the dark\n");
                    break;

                case "2":
                    if (RollStats(user.PlayerSneak, 1, "Sneak") == true)

                    ScrollText("You stand as still as possible only moving slightly to hide your face behind people");
                    ScrollText("You make direct eye contact with him, but luckily he doesn't notice you");

                    user.PlayerSneak = user.StatIncrease(user.PlayerSneak, "Sneak");
                    break;

            }

            Continue();

            ScrollText("You made it away");

            ScrollText("You make your way into the centre of the town, people are everywhere");

            ScrollText("Where will you go?");

            Answer = choice("1. Enter the closest shop you can find \n2. Practice your talking skills with other people (talk) \n3. Try to steal some food for the day (sneak)", 3);

            switch (Answer)
            {
                case "1":
                    currentRoom = shop;
                    currentRoom.Visit();
                    break;
                case "2":
                    ScrollText("\n You walk up to the first normal-ish looking person you can find and chat with them \n they don't reply much but you make them smile a small amount\n you realise it has gotten late.");
                    user.PlayerCharisma = user.StatIncrease(user.PlayerCharisma, "Charm");
                    break;
                case "3":
                    ScrollText("\nYou walk over too a food stand and wait until it gets busy, then try to steal a loaf of bread");
                    RollStats(user.PlayerSneak, 4, "Sneak");
                    ScrollText("The Shopkeep shouts at you and goes to call over the law enforcemnt.. \nYou need to run\n Running through crowds of people makes you tired and once you get away they leave.");
                    user.PlayerSneak = user.StatIncrease(user.PlayerSneak, "Sneak");
                    break;
            }

            ScrollText("Your poor and have to sleep on the street tonight");
            currentRoom = street;

            Continue();

        }

        void Level02()
        {

            currentLevel = Level._02;
            
            Console.Clear();
            ScrollText("Today is a new day and you sleept... ");
            ScrollText("well you slept so thats all that counts right?");
            NewDay();


            ScrollText("Where will you go now?");

            Answer = choice("1. Go to the shop \n2. Try to find a job (charisma) \n3. Try to steal something (sneak)", 3);

            switch(Answer)
            {
                case "1":
                    currentRoom = shop;
                    DescribeRoom(currentRoom);
                    break;
                case "2":
                    ScrollText("You try to find a job");
                    if (RollStats(user.PlayerCharisma, 3, "Charisma"))
                    {
                        ScrollText("You got a job!");
                        user.PlayerCharisma = user.StatIncrease(user.PlayerCharisma, "Charisma");
                        ScrollText("You get 5 gold a day now");
                        DaylyMoney = 5;
                    }
                    else
                    {
                        ScrollText("You didn't get the job");
                    }
                    break;
                case "3":
                    ScrollText("You try to steal something");
                    if (RollStats(user.PlayerSneak, 4, "Sneak"))
                    {
                        ScrollText("You got away with it!");
                        user.PlayerSneak = user.StatIncrease(user.PlayerSneak, "Sneak");
                        ScrollText("You found 3 gold");
                        user.PlayerGold += 3;
                    }
                    else
                    {
                        ScrollText("You got caught and hurt in the process");
                        user.PlayerHealth -= 3;
                        AliveCheck();
                    }

                    break;
            }

            Continue();
        }

        //method to change rooms
        string ChangeRoom()
        {
            currentRoom = street; // starting room 
            DescribeRoom(currentRoom);
            string Answer = choice("Where do you want to go?", 4);
            switch (Answer)
            {
                case "1":
                    if (currentRoom.NorthDoor != null)
                    {
                        currentRoom = currentRoom.NorthDoor;
                        currentRoom.Visit();
                    }
                    else
                    {
                        ScrollText("There is no door that way");
                    }
                    break;
                case "2":
                    if (currentRoom.SouthDoor != null)
                    {
                        currentRoom = currentRoom.SouthDoor;
                        DescribeRoom(currentRoom);
                    }
                    else
                    {
                        ScrollText("There is no door that way");
                    }
                    break;
                case "3":
                    if (currentRoom.EastDoor != null)
                    {
                        currentRoom = currentRoom.EastDoor;
                        DescribeRoom(currentRoom);
                    }
                    else
                    {
                        ScrollText("There is no door that way");
                    }
                    break;
                case "4":
                    if (currentRoom.WestDoor != null)
                    {
                        currentRoom = currentRoom.WestDoor;
                        DescribeRoom(currentRoom);
                    }
                    else
                    {
                        ScrollText("There is no door that way");
                    }
                    break;
            }
            return currentRoom.Name;
        }

        void NewDay() // method for starting a new day 
        {
            ScrollText("A new day begins, your health is restored");
            user.PlayerHealth = 10;
            user.PlayerGold += DaylyMoney;
            ShowStats();
        }

        void DescribeRoom(Room room) // describes the room the player is in 
        {
            ScrollText($"\nYou are now in the {room.Name}.\n{room.Description}\n");

            AsciiImage("ShopSign");

            Console.WriteLine(@"
    Room Exits: {0}{1}{2}{3}",
            room.NorthDoor == null ? "" : "1. North",
            room.SouthDoor == null ? "" : "2. South",
            room.EastDoor == null ? "" : "3. East",
            room.WestDoor == null ? "" : "4. West"
            );
        }

        void ShowStats() // shows the players stats when called
        {
            string PlayerText = "Player Stats";

            ScrollText(CentrePad(PlayerText, 4));
            ScrollText($"Your name is {user.PlayerName}");
            ScrollText($"You are on Level {currentLevel} \n");

            Console.ForegroundColor = ConsoleColor.Green;
            ScrollText($"You have {user.PlayerHealth}HP left.");

            Console.ForegroundColor = ConsoleColor.Blue;
            ScrollText($"sneak level: {user.PlayerSneak}");
            ScrollText($"Intellegence level: {user.PlayerIntellegence}");
            ScrollText($"Charisma level: {user.PlayerCharisma}");
            ScrollText($"Intimidation level: {user.PlayerIntimidation}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            ScrollText($"You have {user.PlayerGold} Gold");
            ScrollText($"You get {DaylyMoney}Gold everyday");
            Console.ResetColor();

            ScrollText(CentrePad("------------", 4));

        }

        string CentrePad(string _text, int amount)
        {
            int textLen = _text.Length;
            return _text.PadLeft(textLen + amount, '-').PadRight(textLen + (amount * 2), '-');
        }

        void Continue() // fast way of waiting for the user to input before clearing the screen 
        {
            PlayerInput("Press enter to Conintue");
            Console.Clear();
        }

        bool AliveCheck() //used to check if player is alive
        {

            if (user.PlayerHealth <= 0)
            {
                isAlive = false;
            }

            if (isAlive == false)
            {

                Console.Clear();
                ScrollText("\nYOU HAVE BEEN CAUGHT\n");
                Console.ReadLine();
                Environment.Exit(0);

            }
            return true;

        }

        bool RollStats(int PlayerStat, int EnemyStat, string Type) // use when Comparing stats from something to see if it is sucessful
        {
            int RanNum = RandomNumber(0, EnemyStat + 1);

            ScrollText(CentrePad("Roll your stats", 3));

            Console.ForegroundColor = ConsoleColor.Blue;
            ScrollText($"\nYou used {Type}");
            ScrollText($"Your {Type} Level is {PlayerStat}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            ScrollText($"They have a Level of {RanNum}");
            for(int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }

            if (PlayerStat >= RanNum)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                ScrollText("You Won Roll\n");
                Console.ResetColor();
                ScrollText(CentrePad("---------------", 3));
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ScrollText("You lost the roll\n");
                Console.ResetColor();
                ScrollText(CentrePad("---------------", 3));
                return false;
            }

            Console.ResetColor();
            ScrollText(CentrePad("---------------", 3));

        }

        void ScrollText(string text) // scrolls through text giving it a nice animation
        {
            Console.WriteLine("");
            for (int i = 0; i < text.Length; i++)
            {
                Thread.Sleep(30);
                Console.Write(text[i]);

            }
            Thread.Sleep(1000);
        }

        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        string choice(string Question, int Max) // asks a question and returns a number between 1 and the max value based on user input
        {
            int Answer = -1;

            do
            {
                try
                {
                    Answer = Convert.ToInt32(PlayerInput(Question + "\n> "));
                }
                catch
                {
                    Console.Write("Input a number\n");
                }

            } while (Answer < 0 || Answer > Max);

            return Convert.ToString(Answer);

        }

        string PlayerInput(string ask) // return what the player inputs while stopping any errors until expected value appears. 
        {
            bool isEntered = false;
            string input = "a";

            do
            {
                try
                {
                    ScrollText(ask);
                    input = Console.ReadLine();

                    if (input.Trim() != "")
                    {
                        isEntered = true;
                    }
                }
                catch
                {
                    Console.WriteLine("Unexpected Value");
                }

                switch(input.Trim().ToLower())
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "showstats":
                        ShowStats();
                        break;
                    case "room":
                        ChangeRoom();
                        break;
                    case "help":
                        ScrollText("Type 'Show Stats' to see your stats \nType 'Room' to see your current room \nType 'Clear' to clear the screen \nType 'Exit' to exit the game\n");
                        break;
                }

            } while (isEntered == false || input == null);

            return input;

        }

        void AsciiImage(string imageName) // method for containing images 
        {

            switch (imageName)
            {
                case "Village":
                    Console.WriteLine(@"
  ~         ~~          __
       _T      .,,.    ~--~ ^^
 ^^   // \                    ~
      ][O]    ^^      ,-~ ~
   /''-I_I         _II____
__/_  /   \ ______/ ''   /'\_,__
  | II--'''' \,--:--..,_/,.-{ },
; '/__\,.--';|   |[] .-.| O{ _ }
:' |  | []  -|   ''--:.;[,.'\,/
'  |[]|,.--'' '',   ''-,.    |
  ..    ..-''    ;       ''. '");

                    break;

                case "ShopSign":
                    Console.WriteLine(@"                       
                   +##############################################################* 
               ##############*##################################################### 
            ###########+         =####=-      :##@@   ...--==@#@. ..  @@#|  ####### 
        ############%-    ####%@   #@@   *@@@   -@@=  :#*#####@#  @*   .@| ####### 
     *#############@=  @@==####%*   @@   ##%@%  #@%   ---...##@#  %@@   .| ####### 
  *##############@@%   @+@'####@@  *@@:         #@@*  +#######@#  ##@@     ####### 
 *################@@:  -@@:@.*#%@  ##@#   +%#####@@*  #######%@#  *##@@%   ######+ 
    ###############@@   +######@  ##%@%   #######@@         +%@@@@#####@@@@%#####- 
       %############@@@         *###@@-   ######@@@@@@@@@@@######################= 
           ##########%@@@@@@@@@%####@@@@@#######@%##########%@@@%%###############  
              ###################################################################");
                    break;

                case "Homeless":
                    Console.WriteLine(@"
▓▓▓▓▓▒▒▒▒▒▒▒▓▒▓▓▓▓▓▓██▒▒▓▒▒▓▓▓▓▓▓▓▓▒▓
▒▓▓▓▓▒▒▒▒▒▒▒▒▓▓██▓▓▓███▓▓▓▓▓▓▓▓▓▓▓▓▓▓
▓▓▓▒▓▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓███▓▓▓▓▓▓▓▓▒▒▓▓▒▒
▒▒▒▓▓▒▒▒▒▒▒▒▒▒▒▓▓███▓███▓▒▓▒▒▒▒▒▓▒▒▒▒
▒▒▓▒▒▒▒▒▒▒▒▒▒▓▓██████████▓▓▓▒▒▒▓▓▒▒▒▒
▒▒▒▒▒▓▒▓▒▒▒▒▓▓▓▓▓▓▓████▓█▓▓▓█▒▒▒▒▒▒▒▒
▒▒▒▒▒▒▒▓▒▒▒▓▓▓▓█▓▓████▓██▓█▓██▓▒▒▒▓▒▒
▒▒▒▒▒▒▒▓▒▒▓██▓▓█▓▓▓███▓▓█▓█▒▓██▓▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▓▓█▓▓█▓█▓▓█▓▓▓█▓▓█▓███▓▓▓▓▒▒
▒▒▒▒▒▒▒▒▓▓█▓██▓█▓▓█▒▓█▓█▓▓█▓███▓▒▓▓▒▓
▒▒▒▒▒▒▒▓▓▓▓▓▓▓████▓▓▓█▓█▓▓█▓▓██▓▒▒▒▒▒
▒▒▒▒▒▒▓▓▓█▓█▓▓███▓▓▒█▓██▓▒▓▓▓██▓▒▒▒▒▓
▒▒▒▒▓▓▒▓███▓▒▒▓▓▓▓▓▒█▓▓▓▓▓▓▓▓███▓▓▒▒▓
▒▒▒▓▓▓▓▓▓▒▓▓▒▒▒▒▓█▒▓█▓▓▓█▓█▓▓███▒▒▒▒▓
▒▒▒▓▒▓▓▒▒▓▓▓▒▓▒▒██▒█▓████▓██▓▓█▓▓▓▒▒▒
▒▒▒▓▓█▓▒███▓▓▓▓██████████▓█▓▓▓█▓▓▓▒▒▒
▒▒▒▓▓▓▒▒█▓▒▒▓████████▓███▓▓▓▒▓█▓▓▓▒▒▒
▒▒▒▒▓▒▒▒▒▒▓██████████▓███▓█▓▓█▓█▓▒▒▒▒
▒▒▒▒▒▓▓▓▓▓██▒▓▓██████▓████▓█████▒▓▒▒▒
▒▒▒▒▒▒▒▒▒▒█▒▓▓▓████████▓█▓█▓▓██▓▒▓▓▒▒
▒▒▒▒▒▒▒▒▒▒▓▒▓▓█▓▓███▓██▓▓██▓▓███▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▒▓▒▓▓▒▒█▓▓▓▓█▓▓▓██▒▓▓▓█▒▒▓▒▒
▒▒▒▒▒▒▒▒▓▓▒▓▓▒▓██▓█▓██▓▓▒█▓▒▓▓▓█▓▒▒▓▒
▒▓▒▒▒▒▒▒▒▓▒▓▓▓██▓█▓▓▓▓▓▓▓█▓▓▓▓▒█▓▓▒▒▒
▓▓▒▒▒▒▒▒▒▓▓▓▓▓▓███▓▒▒▒▓▓▓█▒▒█▓▒▓▓▒▒▒▒
▓▓▓▓▓▓▓▓▓▓███▒▓███▓▒▒▒█▓▓█▒▒████▓▒▒▒▒
▓▓▓▓▓▓▓▓▓█▓▓▓▒█████▒▒▒█▒█▓▓▓█▓██▒▒▒▒▒
▓▓▒▒▓▓▓▓▓▓▓▓▓▓█████▒▒▓▓▒▓▓▒▓▓▒▓█▒▒▒▒▒
▒▒▒▒▒▒▒▒▓▓▓█▓▓██████▒▒▒▒█▓▓█████▒▒▒▒▒
▒▒▒▒▒▒▒▒████▓▓█████▓▓███▓▓█████▒▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▒▓██▓█████████████████▒▓▒▒▒▒
▒▒▒▒▒▒▒▒▓▓▓▓▒▓███▓▒▒▓▓████████▒▒▒▓▒▒▒
▒▒▒▒▒▒▒▒▒▒▒▒▓▒▓▓▓▓▓▒▓▒▒███▒▓▓▒▒▒▒▓▒▒▒
▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▓▓▓█▒▒▓█▒▒▒▒▒▒▒▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓██▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▒▒█▓▓███▒▒▓█▓██▓▒▒▒▒▒▒▒▒▒▒▒▒
▒▓▓▓▒▒▓▓▓▓▓▓▓▓███▓▓▓▓▓███▓▓▓▓▓▓▓▓▓▓▓▓
▒▓▓▓▒▒▒▒▓▓▓▓▓███████▓▓▓██████████▓███
▓▒▓▒▒▓▓▓▒▒▓▓██████▓█▓▓▓▓██▓▓▓▓▓▓████▓
▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▒▒▒▒▒▓▓▓▓▓▓
▒▒▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▒▓▓▓▓▒▒▒
");

                    break;

            }
        }

    }


}
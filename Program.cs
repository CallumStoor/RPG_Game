using RPG_Game;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.CompilerServices;

class Program
{

    enum Level
    {
        Title = 0,
        Intro,
        _01,
        _02
    }

    enum Info
    {
        People = 1,
        Country,
        Dracones
    }

    public static void Main(string[] args)
    {
        // starting stats of the player at the begining of the game
        Player user = new Player("", 10, 1, 1, 1, 1, 0);

        AsciiPictures AsciiImage = new AsciiPictures();

        // rooms in the game
        Room street = new Room("Street",
            "You are standing on a busy street, it smells and everyone is moving past you with somewhere to go.",
            AsciiImage.AsciiDisplay(""),
            true);

        Room shop = new Room("Shop",
            "You are in a small shop, there is a shopkeep behind the counter eyeing you suspiciously",
            AsciiImage.AsciiDisplay("ShopSign"),
            true);

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
        int AvalibleTrade = 2;
        Level currentLevel = Level.Title;

        // Infomation List

        string[] InfoAvalible = 
            {
            "Dracones boss name is John", 
            "Dracones pay off the Goverment",
            "Country has a small rebelian starting again", 
            "Country and it's people are poor",
            "Country has an escape near the east sea",
            "People are getting restless under the rule of Dracones",
            "People fear talk of a new weapon",
            "People are running underground trades without tax from Dracones"
            };

        // game starts here

        Console.ResetColor();

        currentLevel = Level.Title;

        WelcomeScreen();

        if (newGame == true) // if starting a new game run through the intro and levels
        {

            IntroScene();

            Level01();

            Level02();
        }

        AliveCheck(); // check if at end of game alive

        Console.ReadLine();

        void WelcomeScreen() // Infomation about how to play and Level selection
        {
            user.PlayerName = PlayerInput("What is your name: ");

            ScrollText("Type 'help' to see game comands\nYou can Enter Commands any time you are prompted to input\n\n");
            ScrollText(CentrePad("Welcome to Swamp Dewller", 2));

            Answer = choice("What level do you want to start at? \n1. Start New Game \n2. Level 1\n3. Level 2", 3);
            switch (Answer)
            {
                case "1":
                    ScrollText("Starting New Game");
                    newGame = true; // in main allows game to run in order
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
            ScrollText($"Welcome {user.PlayerName}!");

            //show image
            Console.WriteLine(AsciiImage.AsciiDisplay("Village"));

            // shows strings with a wait after 
            ScrollText("This is the great town of ");
            ScrollText("You don't know yet..");

            Continue();

            Console.Clear();
            ScrollTextSlow("In the bustling town of dercher, The dracones are hot on your tail and it's your fault. Jumping into a wagon stationed nearby the wagon then darts off as fast as it can weaving between people and Dracone members");
            Console.WriteLine();
            Thread.Sleep(300);
            ScrollTextSlow($"'Hey {user.PlayerName}..'");
            ScrollTextSlow("'Yeah?'");
            ScrollTextSlow("'What the hell! You said they would be long gone before you got here' \n\n'It's fine they don't even know who you are.' \n\n'Did you foget who we are talking about, even if they couldn't easily find out I would have to change my wagon before they get a full profile on it. every single detail!' \n\n'Well we got away didn't we?' \n\n'No thanks to you' \n\n'Oh just drop it already.' ");
            ScrollTextSlow("'Why are you acting like this wasn't the worst mistake of your life?! At least he is dead now'");
            ScrollTextSlow("'Yeah.. sure..'");
            ScrollTextSlow($"The driver's eyes narrow and glance at {user.PlayerName} for a second\n");
            Continue();

            ScrollTextSlow(CentrePad("A few hours later", 4));
            Console.WriteLine();
            ScrollTextSlow("'You have made it to the next town over and he stops the wagon'\n\n'How about that payment, i know we agreed until we are long gone but after what you done back there i deserve something.' \n");
            Thread.Sleep(500);
            ScrollTextSlow("...\n");
            Thread.Sleep(500);
            ScrollTextSlow("'So about that. I actually got to go, i know crazy right? Anyawy. See ya!'\n");
            Console.ForegroundColor = ConsoleColor.Red;
            ScrollTextSlow("'WHAT YOU CAN'T DO THAT, GET BACK HERE YOU BLITHERING BASTARD!'\n");
            Console.ResetColor();
            Thread.Sleep(200);

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

                    ScrollTextSlow("You stand as still as possible only moving slightly to hide your face behind people");
                    ScrollTextSlow("You make direct eye contact with him, but luckily he doesn't notice you");

                    user.PlayerSneak = user.StatIncrease(user.PlayerSneak, "Sneak");
                    break;

            }

            Continue();

            ScrollText("You made it away");

            ScrollText("You make your way into the centre of the town, people are everywhere");

            ScrollText("Where will you go?");

            Answer = choice("1. Look for places to go \n2. Practice your talking skills with other people (talk) \n3. Try to steal some food for the day (sneak)", 3);

            switch (Answer)
            {
                case "1":
                    ChangeRoom();
                    ScrollText("You are now in the Shop, the shopkeeper doesn't look too fond of that fact, probably because you look like a begger.");

                    Answer = choice("What do you want to do? \n1. Look for places to go \n2. Trade for infomation \n3. Talk to the shopkeep (talk)", 2);
                    break;
                case "2":
                    ScrollText("\n You walk up to the first normal-ish looking person you can find and chat with them \n they don't reply much but you make them smile a small amount\n you realise it has gotten late.");
                    user.PlayerCharisma = user.StatIncrease(user.PlayerCharisma, "Charm");
                    break;
                case "3":
                    ScrollText("\nYou walk over too a food stand and wait until it gets busy, then try to steal a loaf of bread");
                    RollStats(user.PlayerSneak, 4, "Sneak");
                    ScrollText("The Shopkeep shouts at you and goes to call over the law enforcemnt.. \nYou need to run\n Running through crowds of people makes you tired and once you get away they leave. \nYou lose 3 health for that");
                    user.PlayerHealth -= 3;
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

            Answer = choice("1. Look around for places to go \n2. Try to find a job (charisma) \n3. Try to steal something (sneak)", 3);

            switch (Answer)
            {
                case "1":
                    ChangeRoom();
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
            ScrollText($"\nYou are in the {currentRoom.Name}.\n{currentRoom.Description}\n");

            Console.WriteLine(AsciiImage.AsciiDisplay(currentRoom.AsciiText));

            Console.WriteLine(@"
    Room Exits: {0}{1}{2}{3}",
            currentRoom.NorthDoor == null ? "" : $"1. {currentRoom.NorthDoor.Name} Door",
            currentRoom.SouthDoor == null ? "" : $"2. {currentRoom.SouthDoor.Name} Door",
            currentRoom.EastDoor == null ? "" : $"3. {currentRoom.EastDoor.Name} Door",
            currentRoom.WestDoor == null ? "" : $"4. {currentRoom.WestDoor.Name} Door"
            );

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
                        currentRoom.Visit();
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
                        currentRoom.Visit();
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
                        currentRoom.Visit();
                    }
                    else
                    {
                        ScrollText("There is no door that way");
                    }
                    break;
            }
            return currentRoom.Name;
        }

        void InfoShow()
        {
            ScrollText(CentrePad("Infomation Inventory", 3));
            if (user.PlayerInfo != null && user.PlayerInfo.Length != 0)
            {
                ScrollText("\nHere is all the infomation you know. You have a max of 4 that you can hold");
                for (int i = 0; i < user.PlayerInfo.Length; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Convert.ToString(i + 1) + ". ");
                    sb.Append(user.PlayerInfo[i]);
                    ScrollText(Convert.ToString(sb));
                }
                Console.WriteLine();
                ScrollText("That is everything you know right now.");
            }
            else
            {
                ScrollText("You have no infomation stored. use Trade Info to roll for infomation");
            }
        }

        Info GetInfoType(string info) // find and return the type of string when given a InfoAvalible or player info string
        {
            info = info.ToLower().Substring(0, 3);

            if (info == "dra")
            {
                return Info.Dracones;
            }
            else if(info == "cou")
            {
                return Info.Country;
            }
            else if(info == "peo")
            {
                return Info.People;
            }
            else
            {
                ScrollText("Info Type doesn't exist");
                return 0;
            }
        }

        string InfoGain() // Add info to the player randomly chosen
        {
            string item = null;

            int InfoIndex = RandomNumber(0, InfoAvalible.Length);
            if (InfoAvalible != null && user.PlayerInfo.Length < 5)
            {
                string[] newInfo = new string[user.PlayerInfo.Length + 1];
                for (int i = 0; i < user.PlayerInfo.Length; i++)
                {
                    newInfo[i] = user.PlayerInfo[i];
                }
                newInfo[user.PlayerInfo.Length] = InfoAvalible[InfoIndex];
                user.PlayerInfo = newInfo;
                item = InfoAvalible[InfoIndex];
            }
            else if(user.PlayerInfo.Length > 4)
            {
                ScrollText("You already have the max amount of stored info");
            }
            else
            {
                ScrollText("You already have everything");
            }

                return item;
        }

        void InfoTrade() // start a info trade where the player an buy or roll for infomation. can only do this twice a day.
        {
            if(currentRoom.isTradeable == true )
            {
                ScrollText("You look around for people to trade with");

                bool isEnabled = false;
                string Answer2;

                InfoShow();

                while(isEnabled == false)
                {
                    Answer2 = choice("What do you want too do \n1. Sell info \n2. Roll speach for infomation \n3. Exit", 3);

                    int Sell = RandomNumber(10, 30 * Convert.ToInt32(user.PlayerDiscount) + 10);

                    switch (Answer2)
                    {
                        case "1":
                            Answer2 = choice("Input the number of the info u want too sell", user.PlayerInfo.Length);

                            string InfoSelected = user.PlayerInfo[Convert.ToInt32(Answer2) - 1];

                            Info InfoType = GetInfoType(InfoSelected);
                            Sell += (int)InfoType;

                            ScrollText($"You are selling the {InfoSelected} for {Sell} Gold");

                            string confirm = PlayerInput("Yes or no? \n >");
                            confirm = confirm.ToLower().Trim().Substring(0, 1);

                            if (confirm == "y")
                            {
                                ScrollText("Infomation Sold!");
                                user.PlayerGold += Sell;
                            }
                            break;

                        case "2":
                            Answer2 = choice("How do you want to get your info from people? \n1. Have a conversation with someone(Charm) \n2. Threaten someone(Intimidation) \n3. Read infomation (Intellegence) \n4. Listen to others (Sneak)", 4);
                            bool hasWon = false;

                            switch (Answer2)
                            {
                                case "1":
                                    hasWon = RollStats(user.PlayerCharisma, RandomNumber(2, 4), "Charm");
                                    break;

                                case "2":
                                    hasWon = RollStats(user.PlayerIntimidation, RandomNumber(2, 4), "Intimidation");
                                    break;

                                case "3":
                                    hasWon = RollStats(user.PlayerIntellegence, RandomNumber(2, 4), "Intellegence");
                                    break;

                                case "4":
                                    hasWon = RollStats(user.PlayerSneak, RandomNumber(2, 4), "Sneak");
                                    break;

                            }

                        if (hasWon)
                        {
                            InfoGain();
                        }
                        break;



                    default:
                        ScrollText("You exit");
                        break;

                    }
                }
            }
            else
            {
                ScrollText("You have nothing to trade...");
            }

            ScrollText("You stop trading");
        }

        void NewDay() // method for starting a new day 
        {
            ScrollText($"A new day begins, your health is restored to 10 and you have {AvalibleTrade} Infomation Trades");
            AvalibleTrade = 2;
            user.PlayerHealth = 10;
            user.PlayerGold += DaylyMoney;
            ShowStats();
        }

        void ExitGame() // exit the game
        {
            string confirm = PlayerInput("Yes or no? \n >");
            confirm = confirm.ToLower().Trim().Substring(0, 1);

            if (confirm == "y")
            {
                ScrollText("Exiting game");

                Continue();

                Environment.Exit(0);
            }
        }


        void ShowStats() // shows the players stats when called
        {

            ScrollText(CentrePad($"{user.PlayerName}'s Stats", 4));
            ScrollText(CentrePad($"You are in Level {currentLevel}", 1));

            Console.ForegroundColor = ConsoleColor.Green;
            ScrollText($"You have {user.PlayerHealth}HP left.");

            Console.ForegroundColor = ConsoleColor.Blue;
            ScrollText($"sneak level: {user.PlayerSneak}");
            ScrollText($"Intellegence level: {user.PlayerIntellegence}");
            ScrollText($"Charisma level: {user.PlayerCharisma}");
            ScrollText($"Intimidation level: {user.PlayerIntimidation}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            ScrollText($"You have {user.PlayerGold} Gold");

            if(DaylyMoney != 0)
            {
            ScrollText($"You get {DaylyMoney}Gold everyday");
            }

            ScrollText($"Your Discount level is at {user.PlayerDiscount}");
            Console.ResetColor();

            if (AvalibleTrade != 0)
            {
                ScrollText($"You can trade info {AvalibleTrade} more times today");
            }
            else
            {
                ScrollText("You can't Trade anymore infomation today");
            }
            ScrollText("You can use 'ShowInfo' to see your info inventory");

                ScrollText(CentrePad("------------", 4));

        }

        string CentrePad(string _text, int amount)
        {
            int textLen = _text.Length;
            return _text.PadLeft(textLen + amount, '-').PadRight(textLen + (amount * 2), '-');
        }

        void Continue() // fast way of waiting for the user to input before clearing the screen 
        {
            string input = "";

            do
            {
                input = PlayerInput("Press enter to continue\n> ");

            } while (input != "");
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
                ScrollText("\nYOU HAVE BEEN CAUGHT\n\nYour stats were");

                ShowStats();
                Console.ReadLine();
                Environment.Exit(0);

            }
            return isAlive;

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

            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < 3; i++) // add a wait before showing the result
            {
                Thread.Sleep(500);
                Console.Write(" .");
            }

            if (PlayerStat >= RanNum) // check who won the roll and display the result
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

        void ScrollTextSlow(string text) // scrolls through text giving it a nice animation
        {
            Console.WriteLine("");
            for (int i = 0; i < text.Length; i++)
            {
                Thread.Sleep(36);
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
            string input = "";

            do
            {
                try
                {
                    ScrollText(Question + "\n");
                    Console.Write("> ");
                    input = Console.ReadLine().Trim().ToLower();
                }
                catch
                {
                    Console.Write("Input a number\n");
                }

                switch (input)
                {
                    case "exit":
                        ExitGame();
                        break;
                    case "clear":
                        Console.Clear();
                        continue; // re-ask the same question
                    case "show stats":
                        ShowStats();
                        continue;
                    case "room":
                        ScrollText(currentRoom.Description);
                        continue;
                    case "show info":
                        InfoShow();
                        continue;
                    case "trade info":
                        InfoTrade();
                        continue;
                    case "help":
                        ScrollText("You get 2 Trade info per day... etc...");
                        continue;
                }

                try
                {
                    Answer = Convert.ToInt32(input);
                }
                catch
                {
                    Console.WriteLine("Input a number");
                    continue;
                }


                if (Answer >= 1 && Answer <= Max)
                {
                    return Answer.ToString();
                }
                else
                {
                    Console.WriteLine("Input a number between 1 and " + Max);
                }


            } while (true);

            return Convert.ToString(Answer);

        }

        string PlayerInput(string ask) // return what the player inputs while stopping any errors until expected value appears. 
        {
            bool isEntered = false;
            string input = "";

            do
            {
                try
                {
                    ScrollText(ask);
                    input = Console.ReadLine();

                    if (input.Trim() != "" || ask.ToLower() == "press enter to continue\n> ")
                    {
                        isEntered = true;
                    }
                }
                catch
                {
                    Console.WriteLine("Unexpected Value");
                }

                if (currentLevel != Level.Intro && currentLevel != Level.Title)
                {

                    switch (input.Trim().ToLower()) // Check for the players input of other actions
                    {
                        case "exit":
                            ExitGame();
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                        case "show stats":
                            ShowStats();
                            break;
                        case "room":
                            ScrollText(currentRoom.Description);
                            break;
                        case "show info":
                            InfoShow();
                            break;
                        case "trade info":
                            InfoTrade();
                            break;
                        case "help":
                            ScrollText("You get 2 Trade info per day, You can get daily gold by getiing a job \nType 'Show Stats' to see your stats \nType 'Room' to see your current room \nType 'Clear' to clear the screen \nType 'Show Info' Show what infomation you have stored \nType 'Trade Info' to trade info if avalible \nType 'Exit' to exit the game\n");
                            break;
                    }
                }

            } while (isEntered == false || input == null);

            return input;

        }

    }


}
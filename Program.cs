using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;

class Program
{



    public static void Main(string[] args)
    {
        // starting stats of the player at the begining of the game
        Player user = new Player("", 10, 1, 1, 1, 1);

        bool isAlive = true;

        user.PlayerName = PlayerInput("What is your name: ");

        WelcomeScreen();

        IntroScene();

        Level01();

        AliveCheck(); // just in case

        Console.ReadLine();




        void WelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine($"Welcome! {user.PlayerName}");
            //show image
            AsciiImage("Village");
            // shows strings with a wait after 
            ScrollText("This is the great town of !");

            ScrollText("You don't know yet..");

            ScrollText("Anyway, press anything to start");
            Console.ReadKey(true);
            Thread.Sleep(2000);
        }

        void IntroScene()
        {

            Console.Clear();
            Console.WriteLine("Intro to game here");

            Console.WriteLine("press a key to continue");
            Console.ReadKey(true);

        }

        void Level01()
        {
            Console.Clear();

            ScrollText("You flee into the city running as fast as your legs will take you");
            ScrollText("No looking back now.");
            ScrollText("What will you do?");

            string Answer = choice("1. Continue to run away\n2. Hide in the crowds of people (Sneak)", 2);

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
                    ScrollText("\nYou enter the shop and look around, you still have no money... \nYou walk over to the man behind the till and wait in the long line \nWhen you finally reach the man he greets you with a disapointed face, probably becuase you look like a street begger\n\n 'Get out of here I won't give you anything' \n 'No wait' you exclaim 'I need a job!' \n 'Come back another time you aren't the only one' \n\nYou leave the store and look at the setting sun.");
                    break;
                case "2":
                    ScrollText("\n You walk up to the first normal-ish looking person you can find and chat with them \n they don't reply much but you make them smile a small amount\n you realise it has gotten late.");
                    user.PlayerCharisma = user.StatIncrease(user.PlayerCharisma, "Charm");
                    break;
                case "3":
                    ScrollText("\nYou walk over too a food stand and wait until it gets busy, then try to steal a loaf of bread");
                    RollStats(user.PlayerSneak, 4, "Sneak");
                    ScrollText("The Shopkeep shouts at you and goes to call over the law enforcemnt.. \nYou need to run\n Running through crowds of people makes you tired and once you get away they leave.");
                    break;
            }


            ScrollText("Your poor and have to sleep on the street tonight");


            Continue();

        }

        void ShowStats()
        {
            string PlayerText = "Player Stats";

            ScrollText(CentrePad(PlayerText, 4));
            ScrollText($"Your name is {user.PlayerName}");

            Console.ForegroundColor = ConsoleColor.Green;
            ScrollText($"You have {user.PlayerHealth}HP left.");

            Console.ForegroundColor = ConsoleColor.Blue;
            ScrollText($"sneak level: {user.PlayerSneak}");
            ScrollText($"Intellegence level: {user.PlayerIntellegence}");
            ScrollText($"Charisma level: {user.PlayerCharisma}");
            ScrollText($"Intimidation level: {user.PlayerIntimidation}");
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
            ScrollText("Press enter to Conintue");
            Console.ReadLine();
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
            int RanNum = RandomNumber(0, EnemyStat);
            ScrollText($"You used {Type} \nYour {Type} Level is {PlayerStat} \nThey have a perception of {RanNum}");

            if (PlayerStat >= RanNum)
            {
                ScrollText("You Won Roll");
                return true;
            }
            else
            {
                ScrollText("You lost the roll");
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
                    Console.Write("Input a number e.g. 2");
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

                if (input.Trim().ToLower() == "showstat")
                {
                    ShowStats();
                    isEntered = false;

                    ScrollText("Input answer to question");
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
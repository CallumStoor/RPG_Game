using System;



public class Player
{
    // Player Infomation
    public string PlayerName = "Dweller";
    public int PlayerHealth = 10;

    // Player stats
    public int PlayerIntellegence = 1;
    public int PlayerIntimidation = 1;
    public int PlayerCharisma = 1;
    public int PlayerSneak = 1;

    //Player money
    public int PlayerGold = 1;
    public double PlayerDiscount;

    // Player InfoList
    public string[] PlayerInfo;

    public Player(string playerName, int playerHealth, int playerIntellegence, int playerIntimidaton, int playerCharisma, int playerSneak, int playerGold)
    {
        PlayerName = playerName;
        PlayerHealth = playerHealth;
        PlayerCharisma = playerCharisma;
        PlayerIntellegence = playerIntellegence;
        PlayerIntimidation = playerIntimidaton;
        PlayerSneak = playerSneak;
        PlayerGold = playerGold;

    }

    public int StatIncrease(int Stat, string StatType) // display and increase stats 
    {

        int highestStat = Math.Max(PlayerCharisma, PlayerIntimidation);
        PlayerDiscount = highestStat * 0.4f;

        Stat += 1;
        Console.ForegroundColor = ConsoleColor.Blue;
        ScrollText($"Your {StatType} has increased by 1 Level");
        ScrollText($"You are now on {StatType} level {Stat}");

        Console.ResetColor();

        ScrollText($"Your Discount Stat is {highestStat}");

        return Stat;
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
}
